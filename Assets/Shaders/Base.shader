 
 Shader "Archipelago/Base" {
    Properties {
      _Emission ("Emission", Color) = (0,0,0,0)
    }

    SubShader {
      Tags { "RenderType" = "Opaque" }

      CGPROGRAM
      #pragma surface surf Lambert noaddambient

      struct Input {
          float4 color : COLOR;
      };

      fixed3 _Emission;

      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = IN.color + _Emission;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }