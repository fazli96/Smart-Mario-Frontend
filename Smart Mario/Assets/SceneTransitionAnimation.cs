using UnityEngine;

public class SceneTransitionAnimation : MonoBehaviour
{
    public Animator animator;

    public void Fade()
    {
        animator.SetTrigger("FadeOut");
    }
}
