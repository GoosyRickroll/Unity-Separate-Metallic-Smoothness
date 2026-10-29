# Separate Metallic & Smoothness Shader for Unity

A custom shader for Unity's **Built-in Render Pipeline** that allows metallic and smoothness textures to be used separately, with GIMP-style Levels controls.

## Features

- Separate Metallic and Smoothness texture maps
- Metallic Strength slider
- Smoothness Strength slider
- Metallic Levels window (Black Point, Gamma, White Point)
- Smoothness Levels window (Black Point, Gamma, White Point)
- Normal Map with adjustable strength
- Ambient Occlusion Map with adjustable strength
- Albedo texture and color tint

## Requirements

- Unity 2022.3 or later (developed for Unity 2022.3.7f1)
- Built-in Render Pipeline

**Note:** This shader is not designed for URP or HDRP.

## Installation

1. Open Unity.
2. Go to **Window → Package Manager**.
3. Click the **+** button.
4. Select **Add package from git URL**.
5. Paste:

   `https://github.com/GoosyRickroll/Unity-Separate-Metallic-Smoothness.git`

6. Click **Add**.

## How to Use

1. Create a new Material in Unity.
2. Select the shader **Custom/SeparateMetallicSmoothness**.
3. Assign your Albedo, Metallic, Smoothness, Normal, and Occlusion maps.
4. Adjust the Metallic and Smoothness Strength sliders.
5. Click **Metallic Levels...** or **Smoothness Levels...** to open the corresponding adjustment window.
6. Adjust Black Point, Gamma, and White Point to control the appearance of your material
