using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace FitCheck.Editor
{
    public static class PetsFinaleReview
    {
        public static void Build(){
            CampaignChecks.Validate();DropCatalogChecks.Validate();
            string project=Directory.GetParent(Application.dataPath).FullName;
            string root=Directory.GetParent(project).FullName;
            string output=Path.Combine(root,"tools/.pets-finale-build");Directory.CreateDirectory(output);
            string captures=Path.Combine(project,"Captures/PetsFinale");Directory.CreateDirectory(captures);
            File.WriteAllLines(Path.Combine(captures,"solutions.txt"),CampaignChecks.Solutions.Select(fit=>string.Join("|",fit)));
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/FirstCustomer.unity"},locationPathName=Path.Combine(output,"FitCheck.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4HC});
            Debug.Log("PETS_FINALE_BUILD: "+report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Build de revisão falhou");
        }
    }
}
