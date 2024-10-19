#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Interact_Ryu.GetSettingObject;
using System.Reflection;

namespace Interact_Ryu
{

    [CustomEditor(typeof(GetSettingObject))]
    public class ComponentSettingsEditor : Editor
    {

        private List<bool> foldoutStates = new List<bool>();

        public override void OnInspectorGUI()
        {
            // 기본 인스펙터 그리기
            GetSettingObject myComponent = (GetSettingObject)target;

            if (foldoutStates.Count != myComponent.componentSet.Count)
            {
                // foldoutStates 크기가 맞지 않는 경우 재조정
                if (foldoutStates.Count < myComponent.componentSet.Count)
                {
                    foldoutStates.AddRange(new bool[myComponent.componentSet.Count - foldoutStates.Count]);
                }
                else if (foldoutStates.Count > myComponent.componentSet.Count)
                {
                    foldoutStates.RemoveRange(myComponent.componentSet.Count, foldoutStates.Count - myComponent.componentSet.Count);
                }
            }


            for (int i = 0; i < myComponent.componentSet.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");

                foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], myComponent.componentSet[i].settingName);

                if (foldoutStates[i])
                {
                    myComponent.componentSet[i].settingName = EditorGUILayout.TextField("Setting Name", myComponent.componentSet[i].settingName);

                    // 선택된 열거형 표시
                    myComponent.componentSet[i].componentType = (GetSettingObject.ComponentType)EditorGUILayout.EnumPopup("Component Type", myComponent.componentSet[i].componentType);

                    // 선택된 열거형에 따라 다른 필드를 표시
                    if (myComponent.componentSet[i].componentType == GetSettingObject.ComponentType.Animation ||
                        myComponent.componentSet[i].componentType == GetSettingObject.ComponentType.All)
                    {
                        myComponent.componentSet[i].keyValue = EditorGUILayout.TextField("Key", myComponent.componentSet[i].keyValue);
                    }
                    if (myComponent.componentSet[i].componentType.HasFlag(ComponentType.Function) ||
                       myComponent.componentSet[i].componentType.HasFlag(ComponentType.All))
                    {
                        // 스크립트 선택 필드
                        myComponent.componentSet[i].isPlayerMoveAble = EditorGUILayout.Toggle("Toggle", myComponent.componentSet[i].isPlayerMoveAble);
                        myComponent.componentSet[i].script = (MonoBehaviour)EditorGUILayout.ObjectField("Script", myComponent.componentSet[i].script, typeof(MonoBehaviour), true);

                        if (myComponent.componentSet[i].script != null)
                        {
                            // 스크립트의 메서드 목록을 가져옴
                            List<string> methodNames = GetMethodNames(myComponent.componentSet[i].script);

                            if (methodNames.Count > 0)
                            {
                                // 메서드 선택 드롭다운
                                int selectedMethodIndex = methodNames.IndexOf(myComponent.componentSet[i].selectedMethodName);
                                if (selectedMethodIndex == -1) selectedMethodIndex = 0;
                                selectedMethodIndex = EditorGUILayout.Popup("Method", selectedMethodIndex, methodNames.ToArray());
                                myComponent.componentSet[i].selectedMethodName = methodNames[selectedMethodIndex];
                            }
                            else
                            {
                                EditorGUILayout.LabelField("No public methods available.");
                            }
                        }
                    }

                    EditorGUI.indentLevel++;

                    EditorGUILayout.LabelField("Game Objects");
                    /*
                    for (int j = 0; j < myComponent.componentSet[i].gameObjects.Count; j++)
                    {
                        myComponent.componentSet[i].gameObjects[j] = (GameObject)EditorGUILayout.ObjectField($"GameObject {j + 1}", myComponent.componentSet[i].gameObjects[j], typeof(GameObject), true);
                    }
                    */
                    for (int j = 0; j < myComponent.componentSet[i].gameObjects.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        myComponent.componentSet[i].gameObjects[j] = (GameObject)EditorGUILayout.ObjectField($"GameObject {j + 1}", myComponent.componentSet[i].gameObjects[j], typeof(GameObject), true);

                        if (myComponent.componentSet[i].componentType == GetSettingObject.ComponentType.Paint)
                        {
                            myComponent.componentSet[i].paintNames.Add(null);
                            myComponent.componentSet[i].paintNames[j] = EditorGUILayout.TextField(myComponent.componentSet[i].paintNames[j], GUILayout.MinWidth(90), GUILayout.ExpandWidth(true));
                            //myComponent.componentSet[i].paintName[j] = EditorGUILayout.TextField($"이름 {j + 1}", myComponent.componentSet[i].paintName[j]);
                        }

                        /*
                        // 게임 오브젝트 제거 버튼
                        if (GUILayout.Button("Remove", GUILayout.Width(60)))
                        {
                            myComponent.componentSet[i].gameObjects.RemoveAt(j);
                            break; // 항목이 제거되면 for 루프를 빠져나감
                        }
                        */
                        EditorGUILayout.EndHorizontal();
                    }

                    // GameObject 추가 버튼
                    if (GUILayout.Button("Add GameObject"))
                    {
                        myComponent.componentSet[i].gameObjects.Add(null);
                    }
                    // GameObject 제거 버튼
                    if (myComponent.componentSet[i].gameObjects.Count > 0 && GUILayout.Button("Remove Last GameObject"))
                    {
                        myComponent.componentSet[i].gameObjects.RemoveAt(myComponent.componentSet[i].gameObjects.Count - 1);
                    }

                    EditorGUI.indentLevel--;

                    // 항목 삭제 버튼
                    if (GUILayout.Button("Remove Setting"))
                    {
                        myComponent.componentSet.RemoveAt(i);
                        foldoutStates.RemoveAt(i);
                    }

                }
                EditorGUILayout.EndVertical();
            }

            // 새 항목 추가 버튼
            if (GUILayout.Button("Add New Setting"))
            {
                myComponent.componentSet.Add(new ComponentSetting());
                foldoutStates.Add(false);
            }

            // 변경 사항을 저장
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }


        private List<string> GetMethodNames(MonoBehaviour script)
        {
            List<string> methodNames = new List<string>();
            MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                // 매개변수가 없는 메서드만 추가
                if (method.GetParameters().Length == 0)
                {
                    methodNames.Add(method.Name);
                }
            }

            return methodNames;
        }
    }

}
#endif