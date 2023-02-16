using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static T With<T>(this T source, Action<T> mutation)
    {
        mutation(source);
        return source;
    }

    public static T With<T>(this T source, Action<T> mutation, bool condition)
    {
        if (condition)
            mutation(source);
        return source;
    }

    public static T Choose<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        return list[Random.Range(0, list.Count)];
    }

    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        var indices = Enumerable.Range(0, array.Length).ToList();
        foreach (var _ in array)
        {
            var index = Random.Range(0, indices.Count);
            yield return array[indices[index]];
            indices.RemoveAt(index);
        }
    }

    public static void Clear(this Transform source, int skip)
    {
        var i = 0;
        foreach (Transform child in source)
            if (i++ >= skip)
                Object.Destroy(child.gameObject);
    }

    public static T[] Enumerate<T>() => (T[]) Enum.GetValues(typeof(T));

    public static IEnumerator Appear(this CanvasGroup canvasGroup, float fadeDuration)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }

    public static IEnumerator Fade(this CanvasGroup canvasGroup, float fadeDuration)
    {
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, fadeDuration).OnComplete(() => canvasGroup.gameObject.SetActive(false));
        yield return new WaitForSeconds(fadeDuration);
    }
    
    public static IEnumerator FastAppear(this CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        //canvasGroup.alpha = 0;
        yield break;
    }
    
    public static IEnumerator FastFade(this CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(false);
        //canvasGroup.alpha = 1;
        yield break;
    }

    public static void Animate(this Material source, float start, float end, Color glow, float scale, float duration,
        Action onComplete)
    {
        source.SetFloat(AnimationConfiguration.Fade, start);
        source.SetColor(AnimationConfiguration.Glow, glow);
        source.SetFloat(AnimationConfiguration.Scale, scale);
        DOTween.To(() => source.GetFloat(AnimationConfiguration.Fade),
            f => source.SetFloat(AnimationConfiguration.Fade, f),
            end,
            duration
        ).OnComplete(() => onComplete());
    }
}