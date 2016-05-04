using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class MyEditorScript : MonoBehaviour {

    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "AbstracSuperClassTest";
    static string TARGET_DIR = "Builds";

    [MenuItem("Custom/CI/Build WebPlayer")]
    static void PerformWebBuild() {
        string target_dir = APP_NAME + "_WebPlayer";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebPlayer, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build PC")]
    static void PerformPCBuild() {
        string target_dir = APP_NAME + ".exe";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes() {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options) {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        if (res.Length > 0) {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }
}
