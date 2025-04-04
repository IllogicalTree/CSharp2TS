namespace CSharp2TS.CLI.Utility {
    public static class FolderUtility {
        public static string GetRelativeImportPath(string currentFolder, string targetFolder) {
            if (string.Equals(currentFolder, targetFolder, StringComparison.InvariantCultureIgnoreCase)) {
                return "./";
            }

            currentFolder = currentFolder.Replace('\\', '/');
            targetFolder = targetFolder.Replace('\\', '/');

            string relativePath = Path.GetRelativePath(currentFolder, targetFolder).Replace('\\', '/');

            if (!relativePath.StartsWith('.')) {
                relativePath = $"./{relativePath}";
            }

            return $"{relativePath}/";
        }
    }
}
