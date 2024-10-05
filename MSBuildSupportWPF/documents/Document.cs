using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.documents
{
    //Abstract class, which encapsulates all login for navigation and orientation in text

    public abstract class Document
    {
        //List of length of all lines
        public List<int> LengthOfLine { get; } = new List<int>();
        //List with offset on lines' starts
        public List<int> OffsetOnStartLine { get; } = new List<int>();

        protected void calculateCharOnLineFromOffsetOnStartLine()
        {
            LengthOfLine.Clear();
            for (int i = 0; i < OffsetOnStartLine.Count-1; i++)
            {
                LengthOfLine.Add(OffsetOnStartLine[i+1]- OffsetOnStartLine[i]);
            }
        }
        protected void CalculateOffsetOnStartLineFromCharOnLine()
        {
            OffsetOnStartLine.Clear();
            OffsetOnStartLine.Add(0);
            for (int i = 0; i < LengthOfLine.Count; i++)
            {
                OffsetOnStartLine.Add(OffsetOnStartLine[i]+ LengthOfLine[i]);
            }
        }
        public int GetPositionOnStartLine(int line)
        {
            return OffsetOnStartLine[line];
        }
        public int GetLineBasedOnPosition(int position)
        {
            for (int i = 0; i < OffsetOnStartLine.Count - 1; i++)
            {
                if (OffsetOnStartLine[i] <= position && OffsetOnStartLine[i + 1] > position)
                {
                    return i;
                }
            }
            return OffsetOnStartLine.Count-1;
        }

        public int GetStartOfCurrLine(int position)
        {
            return GetPositionOnStartLine(GetLineBasedOnPosition(position));
        }
    }
}
