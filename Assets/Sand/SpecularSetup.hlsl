#ifndef SPECULARSETUP
#define SPECULARSETUP

float SchlickFresnel(float i){
	float x = clamp(1.0-i, 0.0, 1.0);
	float x2 = x*x;
	return x2*x2*x;
}

float GGXGeometricShadowingFunction (float3 light, float3 view , float3 normal, float roughness){

	float NdotL = max( 0 , dot( normal , light));
	float NdotV = max( 0 , dot( normal , view));
	float roughnessSqr = roughness*roughness;
	float NdotLSqr = NdotL*NdotL;
	float NdotVSqr = NdotV*NdotV;


	float SmithL = (2 * NdotL)/ (NdotL + sqrt(roughnessSqr +
( 1-roughnessSqr) * NdotLSqr));
	float SmithV = (2 * NdotV)/ (NdotV + sqrt(roughnessSqr + 
( 1-roughnessSqr) * NdotVSqr));


	float Gs =  (SmithL * SmithV);
	return Gs;
}

float4 FresnelFunction(float3 SpecularColor,float3 light , float3 viewDirection )
{
	float3 halfDirection = normalize( light + viewDirection);
	float power = SchlickFresnel( max( 0 , dot ( light , halfDirection )) );

	return float4( SpecularColor + (1 - SpecularColor) * power , 1 );
}


float MySpecularDistribution( float roughness, float3 lightDir , float3 view , float3 normal , float3 normalDetail, float glossiness)
{
	float3 halfDirection = normalize( view + lightDir);

	float baseShine = pow(  max( 0 , dot( halfDirection , normal  ) ) , 100 / glossiness );
	float shine = pow( max( 0 , dot( halfDirection , normalDetail  ) ) , 10 / roughness )  ;

	return baseShine * shine;
}

void SpecularSetup_half(float3 viewDirection, float3 normalDetail, float3 normal, float3 detail, float4 color, float shininess, float roughness, float glossiness, float4 lightColor, float3 lightDirection, out float4 oceanSpecularColor)
{
	oceanSpecularColor = lightColor * color * 
	MySpecularDistribution ( shininess , lightDirection , viewDirection , normal , detail, glossiness )
	* GGXGeometricShadowingFunction( lightDirection , viewDirection, normal, roughness )
	* FresnelFunction( color , lightDirection , viewDirection)
	/ abs(4 * max( 0.1 , dot( normalDetail , lightDirection )) * max( 0.1 , dot( normalDetail , viewDirection) ) );
}

void SpecularSetup_float(float3 viewDirection, float3 normalDetail, float3 normal, float3 detail, float4 color, float shininess, float roughness, float glossiness, float4 lightColor, float3 lightDirection, out float4 oceanSpecularColor)
{
	oceanSpecularColor = lightColor * color *  
    MySpecularDistribution ( shininess , lightDirection , viewDirection , normal , detail, glossiness )
     * GGXGeometricShadowingFunction( lightDirection , viewDirection, normal, roughness )
     * FresnelFunction( color , lightDirection , viewDirection)
     / abs(4 * max( 0.1 , dot( normalDetail , lightDirection )) * max( 0.1 , dot( normalDetail , viewDirection) ) );
}

#endif