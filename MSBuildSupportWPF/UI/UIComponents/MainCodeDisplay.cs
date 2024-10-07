using MSBuildSupport.code;
using MSBuildSupport.code.codeBlocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Timer = System.Timers.Timer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MSBuildSupport.XML;
using MSBuildSupportWPF.utils;
using MSBuildSupportWPF.UI.tab;
using MSBuildSupportWPF.code.codeBlocks;
using System.Windows.Controls.Primitives;

namespace MSBuildSupportWPF.UI.UIComponents
{
    public class MainCodeDisplay : RichTextBox
    {
        private BlockTree BlockTree { get; }
        private CaretMovement CaretMovement { get; }
        private Timer RebuildTimer { get; }
        private bool loadedForFirstTime = true;
        private ErrorPopup ErrorPopup { get; }

        public MainCodeDisplay(BlockTree blockTree, Timer rebuildTimer, ErrorPopup errorPopup)
        {
            ErrorPopup = errorPopup;
            BlockTree = blockTree;
            BlockTree.MainCodeDisplayAttached = this;
            IsReadOnly = true;
            IsReadOnlyCaretVisible = true;
            Focusable = true;
            Loaded += CodeDisplayLoaded;


            CaretMovement = new CaretMovement(BlockTree.Document);
            RebuildTimer = rebuildTimer;
        }
        private void CodeDisplayLoaded(object sender, RoutedEventArgs e)
        {
            if (loadedForFirstTime)
            {
                var window = Window.GetWindow(this);
                window.PreviewKeyDown += HandleKeyPress;
                loadedForFirstTime = false;
            }

        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            RebuildTimer.Stop();
            int offset = new TextRange(Document.ContentStart, Selection.Start).Text.Length;
            CodeNode currCodeNode = BlockTree.GetNodeOnPosition(offset);

            CaretMovement.MoveCaretBasedOnKey(e.Key, CaretPosition, offset, Document);
            if (e.Key == Key.V && Keyboard.IsKeyDown(Key.LeftCtrl))
            { 
                string text = PasteFromClipboard();
                currCodeNode.InsertString(text, offset - currCodeNode.Position);
                CaretPosition = CaretMovement.SetCursorIndex(offset + text.Length, Document);
            }
            if (e.Key == Key.Back)
            {
                string textSelected = new TextRange(this.Selection.Start, this.Selection.End).Text;
                if (textSelected.Length == 0)
                {
                    currCodeNode.DeleteChar(offset - currCodeNode.Position);
                    CaretPosition = CaretMovement.SetCursorIndex(offset - 1, Document);
                    RebuildTimer.Start();
                }
                else
                {
                    int SelectionStartOffset = new TextRange(Document.ContentStart, Selection.Start).Text.Length;
                    int SelectionEndOffset = new TextRange(Document.ContentStart, Selection.End).Text.Length;

                    BlockTree.DeleteBlockOfText(SelectionEndOffset, SelectionEndOffset- SelectionStartOffset);
                    CaretPosition = CaretMovement.SetCursorIndex(SelectionStartOffset, Document);
                    RebuildTimer.Start();
                }
            }
            char? keyCharNullable = KeyUtils.KeyToChar(e.Key);
            char keyChar;
            if (keyCharNullable == null)
            {
                return;
            }
            else
            {
                keyChar = (char)keyCharNullable;
            }
            Debug.WriteLine(keyChar);
            if (keyChar == '\t')
            {
                
                currCodeNode.InsertString("    ", offset - currCodeNode.Position);
                CaretPosition = CaretMovement.SetCursorIndex(offset + 4, Document);
            }
            else
            {
               
                currCodeNode.InsertChar(keyChar, offset - currCodeNode.Position);
                
                CaretPosition = CaretMovement.SetCursorIndex(offset + 1, Document);
            }
            RebuildTimer.Start();

        }

        public void RebuildAndLoadTree(XMLDocument xMLDocument)
        {
            int offset = new TextRange(Document.ContentStart, Selection.Start).Text.Length;

            XMLValidor xMLValidor = new XMLValidor();
            try
            {
                BlockTree.rebuildFromXMLDocument(xMLDocument);
            }
            catch (System.Xml.XmlException e)
            {
                int offSetPosition = xMLDocument.GetPositionOnStartLine(e.LineNumber - 1) + e.LinePosition - 1;
                CodeNode errorNode = BlockTree.GetLastValibleNode(offSetPosition);
                errorNode.LightAsError(e);
                Debug.WriteLine(e.Message);
                return;
            }

            FlowDocument doc = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            foreach (CodeNode codeNode in BlockTree)
            {
                Run nodeRun = codeNode.CodeRun;
                paragraph.Inlines.Add(nodeRun);
            }
            doc.Blocks.Add(paragraph);
            Document = doc;
            Focus();
            CaretPosition = CaretMovement.SetCursorIndex(offset, Document);
        }
        private string PasteFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                return Clipboard.GetText();
            }
            else
            {
                return string.Empty;
            }
        }
        public void ShowPopup(Exception e, int offset)
        {
            TextPointer startPointer = CaretMovement.SetCursorIndex(offset, this.Document);

            Rect startPointerRect = startPointer.GetCharacterRect(LogicalDirection.Forward);

            Point screenPosition = this.PointToScreen(new Point(startPointerRect.X, startPointerRect.Y));
            ErrorPopup.LoadError(e);

            ErrorPopup.HorizontalOffset = startPointerRect.X+30;
            ErrorPopup.VerticalOffset = startPointerRect.Y- this.ActualHeight+30;

            ErrorPopup.IsOpen = true;
        }
        public void HidePopup()
        {
            ErrorPopup.IsOpen = false;
        }
    }

}
