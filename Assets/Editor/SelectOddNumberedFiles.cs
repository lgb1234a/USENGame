using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class SelectOddNumberedFiles : Editor
{
    [MenuItem("Assets/Select Files Ending with Odd Number")]
    private static void SelectFilesEndingWithOddNumber()
    {
        string folderPath = GetSelectedFolderPath();
        
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogWarning("Please select a folder in the Project window.");
            return;
        }

        // Get all files in the selected folder
        string[] filePaths = Directory.GetFiles(folderPath);

        // Filter files that end with an odd number
        var oddNumberedFiles = filePaths.Where(filePath => 
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            char lastChar = fileName.Last();
            return char.IsDigit(lastChar) && (lastChar - '0') % 2 != 0;
        }).ToArray();

        Object[] objectsToSelect = oddNumberedFiles.Select(filePath => AssetDatabase.LoadAssetAtPath<Object>(filePath)).ToArray();
        Selection.objects = objectsToSelect;

        Debug.Log($"Selected {objectsToSelect.Length} files ending with an odd number.");
    }

    private static string GetSelectedFolderPath()
    {
        Object selectedObject = Selection.activeObject;

        if (selectedObject == null)
            return null;

        // Get the path of the selected asset
        string path = AssetDatabase.GetAssetPath(selectedObject);

        // If the selected asset is a folder, return its path
        // If a file is selected, return its containing folder's path
        if (Directory.Exists(path))
            return path;
        return Path.GetDirectoryName(path);
    }
}
