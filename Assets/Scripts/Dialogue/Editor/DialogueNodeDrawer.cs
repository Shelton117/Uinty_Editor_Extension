using UnityEditor;
using UnityEngine;

namespace Shel.Dialogue
{
    [CustomPropertyDrawer(typeof(DialogueNode))]
    public class DialogueNodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);

            EditorGUI.BeginProperty(position, label, property);

            // 每一行高度一般是 18，但是 20 可以得到更舒服的间距
            var type = property.FindPropertyRelative("Type");
            var sentence = property.FindPropertyRelative("Sentence");
            var avatar = property.FindPropertyRelative("Avatar");
            var optionTitle = property.FindPropertyRelative("OptionTitle");
            var options = property.FindPropertyRelative("Options");
            var eventName = property.FindPropertyRelative("EventName");
            var eventArgs = property.FindPropertyRelative("EventArgs");
            var CharacterName = property.FindPropertyRelative("CharacterName");

            var typePosition = position;
            typePosition.height = 20;
            EditorGUI.PropertyField(typePosition, type);
            position.y += 20;

            if (type.intValue == (int)DialogueNodeType.Text)
            {
                var CharacterNamePosition = position;
                CharacterNamePosition.height = 20;
                EditorGUI.PropertyField(CharacterNamePosition, CharacterName);
                position.y += CharacterNamePosition.height;

                var sentencePosition = position;
                sentencePosition.height = 20;
                EditorGUI.PropertyField(sentencePosition, sentence);
                position.y += sentencePosition.height;
            }
            else if (type.intValue == (int)DialogueNodeType.TextWithAvatar)
            {
                var avatarPosition = position;
                avatarPosition.height = 20;
                EditorGUI.PropertyField(avatarPosition, avatar);
                position.y += avatarPosition.height;

                var CharacterNamePosition = position;
                CharacterNamePosition.height = 20;
                EditorGUI.PropertyField(CharacterNamePosition, CharacterName);
                position.y += CharacterNamePosition.height;

                var sentencePosition = position;
                sentencePosition.height = 20;
                EditorGUI.PropertyField(sentencePosition, sentence);
                position.y += sentencePosition.height;
            }
            else if (type.intValue == (int)DialogueNodeType.Option)
            {
                var optionTitlePosition = position;
                optionTitlePosition.height = 20;
                EditorGUI.PropertyField(optionTitlePosition, optionTitle);
                position.y += optionTitlePosition.height;

                var optionsPosition = position;
                optionsPosition.height = options.arraySize * 20 + 20; // 多加 20 是有加减符号
                EditorGUI.PropertyField(optionsPosition, options);
                position.y += optionsPosition.height;
            }
            else if (type.intValue == (int)DialogueNodeType.CustomEvent)
            {
                var eventNamePosition = position;
                eventNamePosition.height = 20;
                EditorGUI.PropertyField(eventNamePosition, eventName);
                position.y += eventNamePosition.height;

                var eventArgsPosition = position;
                eventArgsPosition.height = eventArgs.arraySize * 20 + 20;
                EditorGUI.PropertyField(eventArgsPosition, eventArgs);
                position.y += eventArgsPosition.height;
            }


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // return base.GetPropertyHeight(property, label);

            if (property.FindPropertyRelative("Type").intValue == (int)DialogueNodeType.Text)
            {
                return 60;
            }

            if (property.FindPropertyRelative("Type").intValue == (int)DialogueNodeType.TextWithAvatar)
            {
                return 80;
            }

            if (property.FindPropertyRelative("Type").intValue == (int)DialogueNodeType.Option)
            {
                return 40 + property.FindPropertyRelative("Options").CountInProperty() * 20 + 20; // 多加 20 是有加减符号
            }

            if (property.FindPropertyRelative("Type").intValue == (int)DialogueNodeType.CustomEvent)
            {
                return 40 + property.FindPropertyRelative("EventArgs").CountInProperty() * 20 + 20; // 多加 20 是有加减符号
            }

            return 20;
        }
    }
}
