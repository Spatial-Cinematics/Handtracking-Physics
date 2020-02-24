using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions {

    public static string Current(this Animator anim) {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
    
    public static string Current(this Animator anim, int layer) {
        return anim.GetCurrentAnimatorClipInfo(layer)[0].clip.name;
    }
    
    public static string Current(this Animator anim, int layer, int infoIndex) {
        return anim.GetCurrentAnimatorClipInfo(layer)[infoIndex].clip.name;
    }

    public static bool CurrentClipIs(this Animator anim, string name) {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    
    public static bool CurrentClipIs(this Animator anim, string name, int layer) {
        return anim.GetCurrentAnimatorStateInfo(layer).IsName(name);
    }

    public static float PlayTime(this Animator anim) {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    public static float PlayTime(this Animator anim, int layer) {
        return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime;
    }
    
}
