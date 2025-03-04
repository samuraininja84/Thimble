using System.Collections.Generic;
using UnityEngine;

namespace Thimble
{
    [System.Serializable]
    public struct NodePointer
    {
        public TextAsset storyFile;
        public string linkName;
        public int linkIndex;
        public List<string> linkNames;

        public void SetStoryFile(TextAsset file)
        {
            // Check if the file is valid, if not return.
            if (file == null) return;
            storyFile = file;
        }

        public void SetLinkName(string name)
        {
            // Check if the name is valid, if not return.
            if (string.IsNullOrEmpty(name)) return;
            if (linkNames == null || linkNames.Count == 0) return;
            if (!linkNames.Contains(name)) return;

            // This method is used to set the link name to the name of the node.
            for (int i = 0; i < linkNames.Count; i++)
            {
                if (linkNames[i] == name)
                {
                    linkName = name;
                    linkIndex = i;
                    break;
                }
            }
        }

        public void SetLinkIndex(int index)
        {
            // Check if the index is valid, if not return.
            if (index < 0 || index >= linkNames.Count) return;
            if (linkNames == null || linkNames.Count == 0) return;
            if (linkNames[index] == null) return;
            if (linkNames[index] == string.Empty) return;

            // This method is used to set the link index to the index of the node.
            if (index >= 0 && index < linkNames.Count)
            {
                linkIndex = index;
                linkName = linkNames[index];
            }
        }

        public List<string> ParseForTitles()
        {
            // This method is used to parse the story file for the title of the node.
            List<string> titles = new List<string>();

            // Intialize the lines with the text from the story file and split it into lines.
            string[] lines = storyFile.text.Split('\n');

            // Loop through the lines and look for the line with "title: {name}".
            for (int i = 0; i < lines.Length; i++)
            {
                // If the line contains "title: {name}", set the nodeName to the name in the line.
                if (lines[i].Contains("title:"))
                {
                    string name = lines[i].Split(':')[1].Trim();
                    titles.Add(name);
                    linkNames.Add(name);
                }
            }

            // If the nodeName is not null, set the currentNodeName to the nodeName.
            return titles;
        }

        public List<string> ParseForContent()
        {
            // Intialize the nodeContent with the parsedContent.
            List<string> nodeContent = new List<string>();
            for (int i = 0; i < ParseForTitles().Count; i++) nodeContent.Add(string.Empty);

            // Intialize the lines with the text from the story file and split it into lines.
            string[] lines = MarkedContent().Split('\n');

            int currentNodeIndex = 0;
            int gap = 0;

            // Loop through the lines and look for the lines 3 lines down from the title line and before the "===" line.
            for (int i = 0; i < lines.Length; i++)
            {
                if (!InContent(i))
                {
                    if (gap == 2)
                    {
                        currentNodeIndex++;
                        Debug.Log("Current Node Index: " + currentNodeIndex);
                        gap = 0;
                    }
                    gap++;
                    continue;
                }

                nodeContent[currentNodeIndex] += lines[i] + "\n";
            }

            // Return the nodeContent.
            return nodeContent;
        }

        public List<int> ParseForContentIndices()
        {
            // This method is used to parse the story file for the lines that doesnt contain title, tags, position, or "---" and "===".
            List<int> nodeIndex = new List<int>();

            // Intialize the lines with the text from the story file and split it into lines.
            string[] lines = MarkedContent().Split('\n');

            // Loop through the lines and skip the lines that are not needed.
            for (int i = 0; i < lines.Length; i++)
            {
                // If the line contains "title:", "---", "tags:", "position", or "===", break the loop, otherwise add the line to the nodeContent.
                if (StartOfContent(lines[i])) continue;
                if (EndOfContent(lines[i])) continue;

                // If the line contains the nodeName, set the currentNodeIndex to the index of the line.
                nodeIndex.Add(i);
            }

            // Return the currentNodeIndex.
            return nodeIndex;
        }

        public string MarkedContent()
        {
            // This method is used to parse the story file for the content of the node.
            string parsedContent = string.Empty;

            // Intialize the lines with the text from the story file and split it into lines.
            string[] lines = storyFile.text.Split('\n');

            // Loop through the lines and look for the lines 3 lines down from the title line and before the "===" line.
            for (int i = 0; i < lines.Length; i++)
            {
                // If the line contains "title:", "---", "tags:", "position", or "===", break the loop, otherwise add the line to the nodeContent.
                if (SkipLine(lines[i])) continue;
                else parsedContent += lines[i] + "\n";
            }

            // Intialize the nodeContent with the parsedContent.
            string nodeContent = string.Empty;

            // After every blank line split the content item.
            string[] splitContent = parsedContent.Split('\n');

            int nodeCount = ParseForTitles().Count;

            for (int i = 0; i < splitContent.Length; i++)
            {
                nodeContent += splitContent[i] + "\n";
            }

            // Return the nodeContent.
            return nodeContent;
        }

        private bool SkipLine(string line)
        {
            // This method is used to skip lines that are not needed.
            // If the line is empty or contains "title:", "---", "tags:", "position", or "===", return true.
            if (string.IsNullOrEmpty(line) || line.Contains("title:") || line.Contains("tags:") || line.Contains("position")) return true;
            return false;
        }

        private bool StartOfContent(string line)
        {
            // This method is used to check if the line is the start of the content.
            // If the line contains "---" return true.
            if (line.Contains("---")) return true;
            return false;
        }

        private bool EndOfContent(string line)
        {
            // This method is used to check if the line is the end of the content.
            // If the line contains "===" return true.
            if (line.Contains("===")) return true;
            return false;
        }

        private bool InContent(int index)
        {
            if (ParseForContentIndices().Contains(index)) return true;
            return false;
        }
    }
}
