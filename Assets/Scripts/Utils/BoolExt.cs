namespace Utils
{
    public static class BoolExt
    {
        public static bool ToBool(this int thisInt)
        {
            if(thisInt == 0) return false;
            
            return true;
        }
        
        public static int ToInt(this bool thisBool)
        {
            return thisBool == true ? 1 : 0;
        }
    }
}