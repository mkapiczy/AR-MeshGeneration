﻿using OpenCVForUnity;
using System;
using UnityEngine;

public struct MatDisplaySettings {

    public static float DEFAULT_SIZE = 0.75f;
    public static MatDisplaySettings FULL_BACKGROUND = new MatDisplaySettings(0,0,2,true);
    public static MatDisplaySettings BOTTOM_LEFT = new MatDisplaySettings(-1, -1, DEFAULT_SIZE, false);
    public static MatDisplaySettings BOTTOM_RIGHT = new MatDisplaySettings(1, -1, DEFAULT_SIZE, false);
    public static MatDisplaySettings TOP_LEFT = new MatDisplaySettings(-1, 1, DEFAULT_SIZE, false);
    public static MatDisplaySettings TOP_RIGHT = new MatDisplaySettings(1, 1, DEFAULT_SIZE, false);

    public float x, y, size;
    public bool background;

    public MatDisplaySettings(float x,float y,float size,bool background)
    {
        this.x = x;
        this.y = y;
        this.size = size;
        this.background = background;
    }

    public MatDisplaySettings(float x,float y,float size) : this(x,y,size,false)
    {
        
    }
}

internal class MatDrawer
{
    public bool active = false;
    public MatDisplaySettings properties;
    public int channels;

    public Texture2D texture = null;

    public void Activate(MatDisplaySettings properties)
    {
        active = true;
        this.properties = properties;
    }

    public void Draw(Material material)
    {
        if (active)
        {
            material.mainTexture = texture;
            material.SetPass(0);

            float height = properties.size;
            float width = height;
            if (texture != null)
                width *= (float)texture.width / texture.height;
            width *= ((float)Screen.height / Screen.width);
            float x = properties.x - width * 0.5f;
            float y = properties.y - height * 0.5f;

            if (x < -1)
                x = -1;
            if (x > 1 - width)
                x = 1 - width;
            if (y < -1)
                y = -1;
            if (y > 1 - height)
                y = 1 - height;

            GL.Begin(GL.TRIANGLE_STRIP);
            GL.Color(Color.white);
            GL.TexCoord(new Vector3(0, 0, 0));
            GL.Vertex(new Vector3(x, y + height, 0));
            GL.Color(Color.white);
            GL.TexCoord(new Vector3(1, 0, 0));
            GL.Vertex(new Vector3(x + width, y + height, 0));
            GL.Color(Color.white);
            GL.TexCoord(new Vector3(0, 1, 0));
            GL.Vertex(new Vector3(x, y, 0));
            GL.Color(Color.white);
            GL.TexCoord(new Vector3(1, 1, 0));
            GL.Vertex(new Vector3(x + width, y, 0));
            GL.End();
        }
    }

}

public class MatDisplay : MonoBehaviour {

    private static MatDisplay instance;

    public static bool SAFE_MODE = false;
    public static bool X86 = false;

    private static bool started = false;
    private static bool error = false;

    private static byte[] convData = new byte[2048 * 2048 * 4];
    private static Mat convertMat;

    private static MatDrawer[] matDisplays = new MatDrawer[64];
    private static MatDrawer[] texDisplays = new MatDrawer[64];
    private static int texDisplayCount = 0;

    private static bool awaitRender = false;

    public Material material;

    private Material foregroundMaterial;
    private Material backgroundMaterial;

    private Camera cam;

    void Start () {
        cam = GetComponent<Camera>();
        if(cam==null)
        {
            cam = GameObject.FindObjectOfType<Camera>();
        }
        if(cam==null)
        {
            Debug.LogError("No camera found");
        }
        if(material==null)
        {
            material = new Material(Shader.Find("AR/MatDisplay"));
        }
        foregroundMaterial = new Material(material);
        foregroundMaterial.renderQueue = 4005;
        backgroundMaterial = new Material(material);
        backgroundMaterial.renderQueue = 500;
        for (int i = 0;i<matDisplays.Length;i++)
        {
            matDisplays[i] = new MatDrawer();
            texDisplays[i] = new MatDrawer();
        }
        convertMat = new Mat();
        Clear();
        started = true;
    }

    public static void DisplayTexture(Texture2D texture,MatDisplaySettings properties)
    {
        MatDrawer uDispl = texDisplays[texDisplayCount++];
        uDispl.texture = texture;
        uDispl.Activate(properties);
    }

    public static bool isValid
    {
        get {
            if (error)
                return false;
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MatDisplay>();
                if (instance == null)
                {
                    Camera cam = Camera.main;
                    if (cam == null)
                    {
                        Debug.LogError("No camera found");
                        error = true;
                        return false;
                    }else
                    {
                        instance = cam.gameObject.AddComponent<MatDisplay>();
                    }
                }
            }
            if (!started)
                return false;
            return true;
        }
    }

    public static void SetCameraFoV(float fov = 41.5f)
    {
        GameObject.FindObjectOfType<Camera>().fieldOfView = fov;
    }

    void Update()
    {
        cam.clearFlags = CameraClearFlags.Depth;
        if (awaitRender)
        {
            Clear();
        }
        awaitRender = true;
    }

    internal static TextureFormat ChannelsToFormat(int channels)
    {
        switch (channels)
        {
            case 1:
                return TextureFormat.RGB24;
            case 3:
                return TextureFormat.RGB24;
            case 4:
                return TextureFormat.RGBA32;
            default:
                throw new Exception("Invalid channel number: "+channels);
        }
    }

    public static unsafe void DisplayImageData(IntPtr ptr, int width,int height,int channels,MatDisplaySettings properties)
    {
        if (!isValid)
            return;
        int len = width * height * channels;

        TextureFormat format = ChannelsToFormat(channels);

        MatDrawer uDispl = null;
        //Reuse displ
        foreach(MatDrawer displ in matDisplays)
        {
            if(!displ.active)
            {
                if (displ.texture != null && format == displ.texture.format && height == displ.texture.height && width == displ.texture.width)
                {
                    uDispl = displ;
                }
            }
        }

        if (uDispl == null)
        {
            //Find new display
            foreach (MatDrawer displ in matDisplays)
            {
                if (displ.texture==null)
                {
                    uDispl = displ;
                    displ.texture = new Texture2D(width,height, format, false);
                    displ.channels = channels;
                    break;
                }
            }
        }
        if (uDispl == null)
        {
            Debug.LogError("Too many mat displays");
            return;
        }

        //Update texture data
        uDispl.texture.LoadRawTextureData(ptr, len);
        uDispl.texture.Apply();

        //Activate display for rendering
        uDispl.Activate(properties);
    }

    public unsafe static void DisplayImageData(byte[] data, int width, int height, int channels, MatDisplaySettings properties)
    {
        int len = width * height * channels;
        if (channels == 1)
        {
            int cI = 0;
            for (int i = 0; i < len; i++)
            {
                byte val = data[i];
                convData[cI++] = val;
                convData[cI++] = val;
                convData[cI++] = val;
            }
            data = convData;
            channels = 3;
        }

        fixed (byte* ptr = data)
        {
            DisplayImageData(new IntPtr(ptr), width, height, channels, properties);
        }
    }

    public static void DisplayMat(Mat mat, MatDisplaySettings properties)
    {
        if (!isValid)
            return;

        Mat uMat = PrepareMatData(mat);
        DisplayImageData(new IntPtr(uMat.dataAddr()), uMat.width(), uMat.height(), uMat.channels(), properties);
    }

    public unsafe static void MatToTexture(Mat srcMat, ref Texture2D targetTexture)
    {
        Mat uMat = PrepareMatData(srcMat);
        TextureFormat format = ChannelsToFormat(uMat.channels());
        if (targetTexture !=null && (targetTexture.width != uMat.width() || targetTexture.height != uMat.height() || targetTexture.format != format || targetTexture.mipmapCount>1))
        {
            Debug.LogWarning("Invalid texture given. Pass uninitialized or valid texture. Deleting and recreating texture.");
            targetTexture = null;
        }
        if (targetTexture==null)
            targetTexture = new Texture2D(srcMat.width(), srcMat.height(), format, false);
        targetTexture.LoadRawTextureData(new IntPtr(uMat.dataAddr()), uMat.width()*uMat.height()*uMat.channels());
        targetTexture.Apply();
    }

    private unsafe static Mat PrepareMatData(Mat mat)
    {
        Mat uMat = mat;
        if (mat.channels() == 1)
        {
            Imgproc.cvtColor(mat, convertMat, Imgproc.COLOR_GRAY2RGB);
            uMat = convertMat;
        }
        return uMat;
    }

    void Clear()
    {
        foreach (MatDrawer matDispl in matDisplays)
        {
            matDispl.active = false;
        }
        foreach (MatDrawer texDisplay in texDisplays)
        {
            texDisplay.texture = null;
            texDisplay.active = false;
        }
        texDisplayCount = 0;
    }

    void OnPreRender()
    {
        if (cam == null)
            return;
        GL.LoadProjectionMatrix(Matrix4x4.identity);
        Matrix4x4 prevModelView = GL.modelview;
        GL.Viewport(new UnityEngine.Rect(0,0,Screen.width,Screen.height));
        GL.modelview = Matrix4x4.identity;
        foreach (MatDrawer displ in matDisplays)
        {
            if (displ.properties.background)
                displ.Draw(backgroundMaterial);
        }
        for (int i = 0; i < texDisplayCount; i++)
        {
            if (texDisplays[i].properties.background)
                texDisplays[i].Draw(backgroundMaterial);
        }
        GL.modelview = prevModelView;
        GL.LoadProjectionMatrix(cam.projectionMatrix);
    }
	
	void OnPostRender ()
    {
        if (cam == null)
            return;
        GL.LoadProjectionMatrix(Matrix4x4.identity);
        Matrix4x4 prevModelView = GL.modelview;
        GL.Viewport(new UnityEngine.Rect(0, 0, Screen.width, Screen.height));
        GL.modelview = Matrix4x4.identity;
        foreach(MatDrawer displ in matDisplays)
        {
            if(!displ.properties.background)
                displ.Draw(foregroundMaterial);
        }
        for(int i=0;i<texDisplayCount;i++)
        {
            if (!texDisplays[i].properties.background)
                texDisplays[i].Draw(foregroundMaterial);
        }
        GL.modelview = prevModelView;
        GL.LoadProjectionMatrix(cam.projectionMatrix);
        Clear();
        awaitRender = false;
    }
}
