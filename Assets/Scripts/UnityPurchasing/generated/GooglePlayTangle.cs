// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("RzuvfRUAceRG+N5nuixJCBIlqJ/ZJDiUE4+unIi424VCvB5SKwvVdAOpvHcOQzE8XAZfWl3YyqL0F6DAuzg2OQm7ODM7uzg4Of37sZDcVHR7grA37NX62RpTX/Spc2oEncxpzDUE4dNtasSDlV2IXGdB40UfgW/D7ImxzTPPovlLN5acaacotwXlRhHHh63UfcjidF+Wx0nKxeTgJMw38vtsaKdo7EveqhUH3bAGY4gp0Xl57w4SdBevQNZI3ziz1WsLckL4050JuzgbCTQ/MBO/cb/ONDg4ODw5Ouw1lD6rxgrfwfx5hmHdNqCntAVb8DGoZd0sg2rFn4qOTw9KUvueufl5q5EDoTwz5gY4tjDFZWn1EWEHLVy2kuD/xeA2sjs6ODk4");
        private static int[] order = new int[] { 10,7,8,7,4,6,11,13,13,13,10,13,12,13,14 };
        private static int key = 57;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
