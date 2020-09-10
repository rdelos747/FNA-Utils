// using Microsoft.Xna.Framework;

// namespace Utils {

//   public class Circlebox : Collider { 

//     public override bool Collides(Hitbox hitbox) {
//       return Collision.RectangleRectangle(
//         Parent.Position + (Position - Origin), 
//         Size, 
//         other.Position + other.BoundsOffset, 
//         other.Size
//       );
//     }

//     public override bool Collides(Circlebox circlebox) {}
//   }
// }