using System.Text.RegularExpressions;

namespace CSharp2TS.CLI.Utility {
    public static class GeneratorUtility {
        public static string GetCleanRouteConstraints(string template) {
            if (string.IsNullOrWhiteSpace(template))
                return template;

            // Matches "{paramName:someConstraint}", capturing "paramName" in a group called "param"
            const string ConstraintPattern = @"\{(?<param>\w+)(?:\:[^}]+)?\}";

            return Regex.Replace(
                template,
                ConstraintPattern,
                match => "${" + match.Groups["param"].Value + "}"
            );
        }
    }
}
