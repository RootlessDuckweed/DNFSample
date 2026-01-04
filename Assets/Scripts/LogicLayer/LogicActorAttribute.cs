using FixMath;
using Sirenix.OdinInspector.Editor.Validation;

namespace LogicLayer
{
    public partial class LogicActor
    {
        protected FixInt level = 1;//等级
        protected string name;//名称
        protected FixInt id;//唯一id
        protected FixInt type;//类型
        
        protected FixInt hp;//血量
        protected FixInt mp;//法力值
        protected FixInt ap;//物理攻击力
        protected FixInt ad;//物理防御
        protected FixInt adDef;//物理防御力
        protected FixInt apDef;//魔法防御力
        protected FixInt pct;//物理暴击率
        protected FixInt mct;//魔法暴击率
        protected FixInt adPctRate;//物理暴击倍率
        protected FixInt apMctRate;//魔法暴击倍率
        protected FixInt str;//力量
        protected FixInt sta;//体力
        protected FixInt Int;//智力
        protected FixInt spi;//精神
        protected FixInt agl;//敏捷
        
        protected FixInt atkRange; //攻击距离，用于区别远程怪物和近战怪物的攻击距离
        protected FixInt searchDisRange;//搜寻距离 用于出生后首次搜寻目标进行进行追击

        #region 战斗时增加的属性
        public FixInt addADDef;
        public FixInt addAPDef;
        public FixInt addAD;
        public FixInt addAP;
        public FixInt addMCT;
        public FixInt addPCT;
        public FixInt addApMCTRate;
        public FixInt addAdPCTRate;
        public FixInt addStr;
        public FixInt addSta;
        public FixInt addInt;
        public FixInt addSpi;
        public FixInt addAgl;
        #endregion

        #region 公开属性
        public FixInt HP=>hp;//血量
        public FixInt MP=>mp;//法力值
        public FixInt AP=>ap+addAP;//物理攻击力
        public FixInt AD=>ad+addAD;//物理防御
        public FixInt AdDef=>adDef+addADDef;//物理防御力
        public FixInt ApDef=>apDef+addAPDef;//魔法防御力
        public FixInt PCT=>pct+addPCT;//物理暴击率
        public FixInt MCT=>mct+addMCT;//魔法暴击率
        public FixInt AdPctRate=>adPctRate+addAdPCTRate;//物理暴击倍率
        public FixInt ApMctRate=>apMctRate+addApMCTRate;//魔法暴击倍率
        public FixInt STR=>str+addStr;//力量
        public FixInt STA=>sta+addSta;//体力
        public FixInt INT=>Int+addInt;//智力
        public FixInt SPI=>spi+addSpi;//精神
        public FixInt AGL=>agl+addAgl;//敏捷
        public FixInt Level => level;
        #endregion

        public void ReduceHP(FixInt reduceHp)
        {
            hp -= reduceHp;
            if (hp <= FixInt.Zero)
            {
                hp = FixInt.Zero;
            }
        }
    }
}
