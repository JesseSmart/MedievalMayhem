Shader "Custom/PearlscentShader"
{
	Properties
	{
		_MainTexture("Texture", 2D) = "white"{}
		//_Color ("Color", Color) = (1, 1, 1, 1)
		_myColour("Example Color", Color) = (1, 1, 1, 1)
		_myEmission("Example Emission", Color) = (1, 1, 1, 1)
		_Range("Range",  Range(0, 10)) = 1

	}

		SubShader{

			CGPROGRAM
			#pragma surface surf Lambert


			sampler2D _MainTexture;
			fixed4 _myColour;
			fixed4 _myEmission;
			half _Range;


			struct Input
			{
				float2 uv_MainTexture;
			};


			void surf(Input IN, inout SurfaceOutput o) 
			{
	
				o.Albedo = (tex2D(_MainTexture, IN.uv_MainTexture) * _Range + _myColour.rgb).rgb;

				o.Emission = _myEmission.rgb;
			}


			ENDCG


		}

		FallBack "Diffuse"



}
