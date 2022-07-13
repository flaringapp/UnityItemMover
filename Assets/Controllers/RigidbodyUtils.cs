using UnityEngine;

namespace Controllers
{
    public static class RigidbodyUtils
    {
        public static void LimitSpeed(this Rigidbody rigidbody, float maxSpeed)
        {
            var brakeVelocity = rigidbody.velocity.LimitSpeedBreakVelocity(maxSpeed);
            rigidbody.AddForce(brakeVelocity);
        }

        public static Vector3 LimitVelocitySpeed(this Vector3 velocity, float maxSpeed)
        {
            return velocity + velocity.LimitSpeedBreakVelocity(maxSpeed);
        }

        public static Vector3 LimitSpeedBreakVelocity(this Vector3 velocity, float maxSpeed)
        {
            var speed = Vector3.Magnitude(velocity);
            if (speed <= maxSpeed) return Vector3.zero;

            var brakeSpeed = speed - maxSpeed;
            return -velocity.normalized * brakeSpeed;
        }
    }
}