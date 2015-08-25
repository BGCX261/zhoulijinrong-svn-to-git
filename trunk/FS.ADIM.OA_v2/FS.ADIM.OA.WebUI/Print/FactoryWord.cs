
namespace WordMgr
{
    public class FactoryWord
    {
        // WORD工厂模式 构造WORD对象实例
        public static IWord CreateWordObj()
        {
            return (new Word07());
        }
    }
}
