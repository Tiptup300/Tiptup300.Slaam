﻿using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.States.Match.Powerups;

public class PowerupManager
{
   public static PowerupManager Instance { get; private set; } = new PowerupManager(ServiceLocator.Instance.GetService<IResources>());

   private Random rand = new Random();
   private readonly IResources _resources;

   public PowerupManager(IResources resources)
   {
      _resources = resources;
   }

   public PowerupType GetRandomPowerup()
   {
      return (PowerupType)rand.Next(1, 5);
   }

   public Texture2D GetPowerupTexture(PowerupType type)
   {
      switch (type)
      {
         case PowerupType.SpeedUp:
            return _resources.GetTexture("SpeedUp0").Texture;

         case PowerupType.SpeedDown:
            return _resources.GetTexture("SpeedDown0").Texture;

         case PowerupType.Inversion:
            return _resources.GetTexture("Inversion0").Texture;

         case PowerupType.Slaam:
            return _resources.GetTexture("Slaam0").Texture;

         default:
            throw new Exception();

      }
   }
}
