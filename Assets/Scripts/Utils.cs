public static class Utils
{
    public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float proportion = (value - fromMin) / (fromMax - fromMin);
        float mappedValue = toMin + (proportion * (toMax - toMin));
        return mappedValue;    
    }
}