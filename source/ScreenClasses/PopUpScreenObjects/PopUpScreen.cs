using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Metal_Jewel.ManagerClasses;
using Metal_Jewel.SpriteObjects.SpriteObjects;
using Metal_Jewel.ScreenClasses.MenuScreenObjects;
using Metal_Jewel.Effects;

namespace Metal_Jewel.ScreenClasses
{
    //Detta är superklassen för samtliga screen-objekt som ska fungera som en PopUp-skärm.
    public abstract class PopUpScreen : MenuScreen
    {
        //Alla PopUp-skärmar kommer att ha en bakgrundsskugga.
        protected Sprite backgroundShade;
        //Bakgrundsskuggan kommer att ha en varierande alpha med hjälp av ett alphaAnimationEffect-objekt.
        protected AlphaAnimationEffect alphaAnimationEffect;
        //Variabel som håller reda på bakgrundsskuggans färg/alpha.
        protected Color effectColor;

        //Vid anrop måste man ange vilken screenManager som ska hantera screen-objektet.
        public PopUpScreen(ScreenManager screenManager)
            : base(screenManager)
        {
        }

        //Variabler som inte kunde initieras i konstruktorn initieras här.
        public override void Initialize()
        {
            base.Initialize();
            effectColor = backgroundShade.Color;
            alphaAnimationEffect = new AlphaAnimationEffect(EffectStatus.IncreaseEffect, ref effectColor, 15, 0, 128);
            backgroundShade.Color = effectColor;
        }

        //Den här metoden ser till att spelaren bara kan trycka på objektets knappar om skuggans alpha har nått sin maxnivå.
        public override void HandleInput(InputManager inputManager)
        {
            //Är skuggans alpha på sin maxnivå? Då kan spelaren trycka på objektets knappar.
            if (alphaAnimationEffect.EffectStatus == EffectStatus.EffectAtMax)
                base.HandleInput(inputManager);

            //Har spelaren klickat på någon knapp och är inte bakgrunden genomskinlig? Då ska alpha-effekten minskas.
            if (selected != null && alphaAnimationEffect.EffectStatus != EffectStatus.EffectAtMin)
                alphaAnimationEffect.startDecreaseEffect();
        }

        //Metoden kallar på basklassens Update-metod och metoden uppdaterar även alpha-effekten.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            alphaAnimationEffect.Update(ref effectColor);
            backgroundShade.Color = effectColor;
        }

        //Metoden målar upp det som är unikt för det här screen-objektet men även dess knappar och bakgrund.
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Bakgrund och knappar målas först upp.
            base.Draw(spriteBatch);
            //Bakgrundsskuggan målas sedan upp.
            backgroundShade.Draw(spriteBatch);
        }
    }
}
