struct VSInput {
    float3 POSTION : POSTION;
    float4 COLOR : COLOR;
};

struct VSOutput {
    float4 POSTION : SV_Position;
    float4 COLOR : COLOR;
};

VSOutput VSMain(VSInput input) {
    VSOutput output;
    output.POSTION = float4(input.POSTION, 1.0f);
    output.COLOR = input.COLOR;
    return output;
}

float4 PSMain(VSOutput input) : SV_TARGET {
    return input.COLOR;
}