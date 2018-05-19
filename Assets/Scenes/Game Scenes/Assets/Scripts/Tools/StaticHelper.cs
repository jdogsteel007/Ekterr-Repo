using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Devin) Here are some functions I ripped from another project of mine that might come in handy
/// </summary>
public static class StaticHelper
{
    /// <summary>
    /// Pauses for an amount of frames (coroutine)
    /// </summary>
    public static IEnumerator FramePause(int frames)
    {
        Time.timeScale = 0;
        for (int i = 0; i < frames; i++) //pause for i frames
        {
            yield return null;
        }
        Time.timeScale = 1;
    }

    /// <summary>
    /// Pauses for an amount of frames and calls a function when done (coroutine)
    /// </summary>
    public static IEnumerator FramePause(int frames, System.Action callback) //callback is called after frames are done
    {
        Time.timeScale = 0;
        for (int i = 0; i < frames; i++) //pause for i frames
        {
            yield return null;
        }
        Time.timeScale = 1;
        if (callback != null)
            callback();
    }

    /// <summary>
    /// 2D version of the lookat function
    /// </summary>
    public static Quaternion LookAt2D(Vector3 source, Vector3 target)
    {
        Vector3 diff = target - source;
        diff.Normalize();
        float zAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, zAngle - 90);
    }

    /// <summary>
    /// Snaps the gameobject to the pixel grid pased on its SpriteRenderer and PPU
    /// </summary>
    public static Vector3 SnapToPixelGrid(Transform target)
    {
        SpriteRenderer sp = target.gameObject.GetComponent<SpriteRenderer>();
        if (sp)
        {
            float PPU = sp.sprite.pixelsPerUnit;
            Vector3 offset = Vector3.zero, final = target.position * PPU;

            offset += target.up * (sp.sprite.rect.height % 2);
            offset += target.right * (sp.sprite.rect.width % 2);
            offset = AbsVec3(offset);
            offset *= 0.5f;

            final = new Vector3(Mathf.Floor(final.x), Mathf.Floor(final.y), final.z) + offset;
            final /= PPU;

            return final;
        }
        return target.position;
    }

    /// <summary>
    /// Snaps a 9 slice object's scale to the pixel grid based on PPU
    /// </summary>
    public static void Snap9SlicePixelScale(SpriteRenderer target, int ppu)
    {
        Vector2 calc = target.size * ppu;
        calc.x -= calc.x % 1;
        calc.y -= calc.y % 1;
        if ((calc.x % 2) != 0) calc.x += 1;
        if ((calc.y % 2) != 0) calc.y += 1;
        target.size = calc / ppu;
    }
    public static Vector3 AbsVec3(Vector3 input)
    {
        return new Vector3(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
    }
    public static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
    public static float Mod(float a, float b)
    {
        return (a % b + b) % b;
    }

    /// <summary>
    /// Draw a 2d debug box widget in the scene editor
    /// </summary>
    public static void Draw2DBoxWidget(Vector3 Center, Vector2 Size, float angle)
    {
        float n = 90 * Mathf.Deg2Rad;
        float a = (angle * Mathf.Deg2Rad) + n;
        Vector3 last = Center;
        Vector3 next = new Vector3();
        Vector3 Up = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * Size.y;
        Vector3 Left = new Vector3(Mathf.Cos(a + n), Mathf.Sin(a + n)) * Size.x;

        last += (Left - Up) * 0.5f;
        next = last + Up;
        Debug.DrawLine(last, next);
        last = next;
        next -= Left;
        Debug.DrawLine(last, next);
        last = next;
        next -= Up;
        Debug.DrawLine(last, next);
        last = next;
        next += Left;
        Debug.DrawLine(last, next);
    }
}
