 
 Shader "Archipelago/Base Unlit Alpha" {
    Properties {
      _Emission ("Emission", Color) = (0,0,0,0)
      _Transparency ("Transparency", Range(0, 1)) = 0
    }


    SubShader {
      Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
      LOD 100

      CGPROGRAM
      #pragma surface surf Unlit alpha noforwardadd noambient

      half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten) {
          half4 c;
          c.rgb = s.Albedo;
          c.a = s.Alpha;
          return c;
      }

      struct Input {
          float4 color : COLOR;
      };

      fixed3 _Emission;
      half _Transparency;

      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = IN.color;
          o.Alpha = 1.0 - _Transparency;
          o.Emission = _Emission;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }
  