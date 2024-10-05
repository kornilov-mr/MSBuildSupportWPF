using MSBuildSupport.code;
using MSBuildSupportWPF.documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace MSBuildSupportWPF.UI.tab
{
    internal class CaretMovement
    {
        private Document Document { get; }

        public CaretMovement(Document document)
        {
            Document = document;
        }

        public void MoveCaretBasedOnKey(Key key, TextPointer caret, int startOffSet, FlowDocument FlowDocument)
        {
            switch (key)
            {
                case Key.Left:
                    MoveCaretLeft(caret, startOffSet, FlowDocument);
                    break;
                case Key.Right:
                    MoveCaretRight(caret, startOffSet, FlowDocument);
                    break;
                case Key.Up:
                    MoveCaretUP(caret, startOffSet, FlowDocument);
                    break;
                case Key.Down:
                    MoveCaretDown(caret, startOffSet, FlowDocument);
                    break;
            }
        }
        private void MoveCaretUP(TextPointer caret, int startOffSet, FlowDocument FlowDocument)
        {
            int currLine = Document.GetLineBasedOnPosition(startOffSet);
            if (currLine == 0)
            {
                return;
            }
            int startOfCurrLine = Document.GetStartOfCurrLine(startOffSet);

            int sizeOfPrevLine = startOfCurrLine - Document.GetPositionOnStartLine(currLine - 1);

            int positionOnCurrLine = startOffSet - startOfCurrLine;

            int newOffset;
            if (positionOnCurrLine > sizeOfPrevLine)
            {
                newOffset = startOfCurrLine - 1;
            }
            else
            {
                newOffset = startOfCurrLine - (sizeOfPrevLine - positionOnCurrLine);
            }
            caret = SetCursorIndex(newOffset, FlowDocument);
        }
        private void MoveCaretDown(TextPointer caret, int startOffSet, FlowDocument FlowDocument)
        {
            int currLine = Document.GetLineBasedOnPosition(startOffSet);
            if (currLine == Document.LengthOfLine.Count)
            {
                return;
            }
            int startOfCurrLine = Document.GetStartOfCurrLine(startOffSet);

            int sizeOfNextLine = Document.GetPositionOnStartLine(currLine + 1) - startOfCurrLine;

            int positionOnCurrLine = startOffSet - startOfCurrLine;

            int newOffset;
            if (positionOnCurrLine > sizeOfNextLine)
            {
                newOffset = Document.GetPositionOnStartLine(currLine + 2) - 1;
            }
            else
            {
                newOffset = Document.GetPositionOnStartLine(currLine + 1) + (sizeOfNextLine - positionOnCurrLine);
            }
            caret = SetCursorIndex(newOffset, FlowDocument);
        }
        private void MoveCaretLeft(TextPointer caret, int startOffSet, FlowDocument FlowDocument)
        {
            caret = SetCursorIndex(startOffSet - 1, FlowDocument);

        }
        private void MoveCaretRight(TextPointer caret, int startOffSet, FlowDocument FlowDocument)
        {
            caret = SetCursorIndex(startOffSet + 1, FlowDocument);

        }
        public virtual TextPointer SetCursorIndex(int offset, FlowDocument FlowDocument)
        {
            TextPointer CaretPosition;
            if (offset <= 0)
            {
                CaretPosition = FlowDocument.ContentStart;
                return CaretPosition;
            }

            string fullText = new TextRange(FlowDocument.ContentStart, FlowDocument.ContentEnd).Text;
            int totalTextLength = fullText.Length;
            if (offset >= totalTextLength)
            {
                CaretPosition = FlowDocument.ContentEnd;
                return CaretPosition;
            }

            TextPointer endPtr = FlowDocument.ContentStart
                .GetPositionAtOffset(offset, LogicalDirection.Forward);
            TextRange range = new TextRange(FlowDocument.ContentStart, endPtr);
            int diff = offset - range.Text.Length;
            while (diff != 0)
            {
                endPtr = endPtr.GetPositionAtOffset(diff, LogicalDirection.Forward);
                range = new TextRange(FlowDocument.ContentStart, endPtr);
                diff = offset - range.Text.Length;

                if (diff < 0)
                {
                    endPtr = FlowDocument.ContentEnd;
                    break;
                }
            }

            CaretPosition = endPtr;
            return CaretPosition;
        }

    }
}
