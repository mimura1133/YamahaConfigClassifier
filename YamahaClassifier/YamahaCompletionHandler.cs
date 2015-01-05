using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{
    [Export(typeof (IVsTextViewCreationListener))]
    [ContentType("yamahaconf")]
    [Name("YamahaCompletion")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class YamahaCompletionHandlerProvider : IVsTextViewCreationListener
    {
        [Import] internal IVsEditorAdaptersFactoryService AdapterService;

        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }

        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            Func<YamahaCompletionCommandHandler> createCommandHandler =
                delegate { return new YamahaCompletionCommandHandler(textViewAdapter, textView, this); };
            textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
        }
    }

    internal class YamahaCompletionCommandHandler : IOleCommandTarget
    {
        private readonly IOleCommandTarget _nextCommandHandler;
        private readonly YamahaCompletionHandlerProvider _provider;
        private readonly ITextView _textView;
        private ICompletionSession _session;

        public YamahaCompletionCommandHandler(IVsTextView TextViewAdapter, ITextView TextView,
            YamahaCompletionHandlerProvider Provider)
        {
            _textView = TextView;
            _provider = Provider;


            //add the command to the command chain
            TextViewAdapter.AddCommandFilter(this, out _nextCommandHandler);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return QueryStatusGotoDefinition(pguidCmdGroup, prgCmds)
                ? VSConstants.S_OK
                : _nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (VsShellUtilities.IsInAutomationFunction(_provider.ServiceProvider))
            {
                return _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }
            var commandID = nCmdID;
            var typedChar = char.MinValue;

            if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint) VSConstants.VSStd2KCmdID.TYPECHAR)
            {
                typedChar = (char) (ushort) Marshal.GetObjectForNativeVariant(pvaIn);
            }

            if (nCmdID == (uint) VSConstants.VSStd2KCmdID.RETURN
                || nCmdID == (uint) VSConstants.VSStd2KCmdID.TAB)
            {
                //check for a a selection
                if (_session != null && !_session.IsDismissed)
                {
                    //if the selection is fully selected, commit the current session
                    if (_session.SelectedCompletionSet.SelectionStatus.IsSelected)
                    {
                        _session.Commit();
                        //also, don't add the character to the buffer
                        return VSConstants.S_OK;
                    }
                    //if there is no selection, dismiss the session
                    _session.Dismiss();
                }
            }

            var retVal = _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            var handled = false;
            if (!typedChar.Equals(char.MinValue) && (char.IsLetter(typedChar) || typedChar == ' '))
            {
                if (_session == null || _session.IsDismissed) // If there is no active session, bring up completion
                {
                    TriggerCompletion();
                    if (_session != null)
                        _session.Filter();
                }
                else //the completion session is already active, so just filter
                {
                    _session.Filter();
                }
                handled = true;
            }
            else if (commandID == (uint) VSConstants.VSStd2KCmdID.BACKSPACE //redo the filter if there is a deletion
                     || commandID == (uint) VSConstants.VSStd2KCmdID.DELETE)
            {
                if (_session != null && !_session.IsDismissed)
                    _session.Filter();
                handled = true;
            }
            if (handled) return VSConstants.S_OK;

            return retVal;
        }

        private bool QueryStatusGotoDefinition(Guid pguidCmdGroup, OLECMD[] prgCmds)
        {
            if (pguidCmdGroup != VSConstants.GUID_VSStandardCommandSet97) return false;

            switch ((VSConstants.VSStd97CmdID) prgCmds[0].cmdID)
            {
                case VSConstants.VSStd97CmdID.GotoDefn:
                    prgCmds[0].cmdf = (uint) OLECMDF.OLECMDF_ENABLED | (uint) OLECMDF.OLECMDF_SUPPORTED;
                    return true;
            }

            return false;
        }

        private void TriggerCompletion()
        {
            //the caret must be in a non-projection location 
            var caretPoint =
                _textView.Caret.Position.Point.GetPoint(x => (true), PositionAffinity.Predecessor);

            if (!caretPoint.HasValue) return;

            _session = _provider.CompletionBroker.CreateCompletionSession
                (_textView,
                    caretPoint.Value.Snapshot.CreateTrackingPoint(caretPoint.Value.Position, PointTrackingMode.Positive),
                    true);

            //subscribe to the Dismissed event on the session
            _session.Dismissed += OnSessionDismissed;
            _session.Start();
        }

        private void OnSessionDismissed(object sender, EventArgs e)
        {
            _session.Dismissed -= OnSessionDismissed;
            _session = null;
        }
    }
}