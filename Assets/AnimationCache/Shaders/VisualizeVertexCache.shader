Shader "Unlit/VisualizeVertexCache"
{
	Properties
	{
		_VCount("vertexCount",Float) = 0
		_TCount("triangles count", Float) = 0
		_Keyframes("keyframeCount",Float) = 0
		_AnimLength("clip length",Float) = 0
		_MainTex("Texture", 2D) = "white" {}
	}
		CGINCLUDE
#include "UnityCG.cginc"
#include "Assets/CGINC/Quaternion.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD1;
			float triID : TEXCOORD2;
			int iid : TEXCOORD3;
			float4 vertex : SV_POSITION;
		};

		struct iData {//instance data
			float3 position;
			float3 velocity;
			float4 rotation;
			float animTime;
			float tori;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		float _VCount, _TCount, _Keyframes, _AnimLength;

		StructuredBuffer<iData> _IData;
		StructuredBuffer<uint> _Indices;
		StructuredBuffer<float2> _UV;
		StructuredBuffer<float3> _VertexData;
		StructuredBuffer<float3> _NormalsData;

		v2f vert(uint vid : SV_VertexID, uint iid : SV_InstanceID)
		{
			iData i = _IData[iid];
			float t = fmod(iid + _Time.y * _Keyframes, _Keyframes);
			float f0 = floor(t);
			float f1 = fmod(f0 + 1, _Keyframes);
			t = frac(t);

			uint idx = _Indices[vid];
			float2 uv = _UV[idx];
			float4 pos = float4(lerp(_VertexData[idx + _VCount*f0], _VertexData[idx + _VCount*f1], t), 1.0);
			float3 normal = lerp(_NormalsData[idx + _VCount*f0], _NormalsData[idx + _VCount*f1], t);

			pos.xyz = rotateWithQuaternion(pos.xyz, i.rotation);
			pos.xyz += i.position;
			normal = rotateWithQuaternion(normal, i.rotation);

			v2f o;
			o.vertex = mul(_Object2World, pos);
			o.normal = normal;
			o.triID = floor(vid / 3);
			o.uv = uv;
			o.iid = iid;
			return o;
		}

		[maxvertexcount(3)]
		void geom(triangle v2f v[3], inout TriangleStream<v2f> triStream)
		{
			float val = sin(_Time.z + v[0].iid);
			float id = v[0].triID;
			float t = (id + 1) / _TCount;

			float size = (length(v[0].vertex.xyz - v[1].vertex.xyz) + length(v[1].vertex.xyz - v[2].vertex.xyz) + length(v[2].vertex.xyz - v[0].vertex.xyz)) / 6;
			float3 center = float3(sin(id), 0, cos(id))*sqrt(t);
			float3 normal = normalize(center);

			normal = rotateAngleAxis(normal, normalize(cross(normal, float3(0, 1, 0))), _Time.y + id * 100);

			float3 tangent = float3(0, 1, 0);

			for (int i = 0; i < 3; i++) {
				float3 p0 = v[i].vertex.xyz;
				float3 p1 = center + size*rotateAngleAxis(tangent, normal, -i*2.0 / 3.0*UNITY_PI + _Time.y*(1.0 + t)) + _IData[v[i].iid].position;

				v[i].vertex.xyz = lerp(p0, p1, saturate(val));
				v[i].vertex = mul(UNITY_MATRIX_VP, v[i].vertex);
				v[i].normal = lerp(v[i].normal, normal, saturate(val));

				triStream.Append(v[i]);
			}
		}

		half4 frag(v2f i) : SV_Target
		{
			return half4(i.normal*0.5 + 0.5,1);
			half diff = 0.5 + dot(i.normal, float3(0,1,0));
			half4 col = tex2D(_MainTex, i.uv);
			if (col.a < 0.5)discard;
			return col * diff;
		}
			ENDCG
			SubShader
		{
			Tags{ "RenderType" = "Opaque" }
				LOD 100

				Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				ENDCG
			}
		}
}
