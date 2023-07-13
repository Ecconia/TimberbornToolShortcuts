using HarmonyLib;
using Timberborn.InputSystem;

namespace ToolShortcuts.InputSystem
{
    class InputServicePatch
    {
        [HarmonyPatch(typeof(InputService), "ChangeGameSpeed", MethodType.Getter)]
        public static class PatchChangeGameSpeed
        {
            private static void Postfix(ref int? __result, InputService __instance)
            {
                if (__result > 0 && !__instance.IsAltHeld)
                {
                    __result = null;
                }
            }
        }
    }
}