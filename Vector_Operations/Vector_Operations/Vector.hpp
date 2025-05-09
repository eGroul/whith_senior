#pragma once

#include <cmath>

struct Vector3
{
	float x;
	float y;
	float z;

	Vector3() : x(0.0f), y(0.0f), z(0.0f) {}

	Vector3(float _x, float _y, float _z)
		: x(_x), y(_y), z(_z)
	{
	}

	Vector3(const Vector3& other)
		: x(other.x), y(other.y), z(other.z)
	{
	}

	Vector3& operator=(const Vector3& rhs)
	{
		x = rhs.x;
		y = rhs.y;
		z = rhs.z;
		return *this;
	}

	Vector3 operator+(const Vector3& rhs) const
	{
		return Vector3(x + rhs.x, y + rhs.y, z + rhs.z);
	}

	Vector3 operator-(const Vector3& rhs) const
	{
		return Vector3(x - rhs.x, y - rhs.y, z - rhs.z);
	}

	Vector3 operator*(float scalar) const
	{
		return Vector3(x * scalar, y * scalar, z * scalar);
	}

	Vector3 operator/(float scalar) const
	{
		if (scalar != 0.0f)
		{
			return Vector3(x / scalar, y / scalar, z / scalar);
		}
		return *this; // Return without change if division by zero
	}

	Vector3& operator+=(const Vector3& rhs)
	{
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}

	Vector3& operator-=(const Vector3& rhs)
	{
		x -= rhs.x;
		y -= rhs.y;
		z -= rhs.z;
		return *this;
	}

	float Dot(const Vector3& rhs) const
	{
		return x * rhs.x + y * rhs.y + z * rhs.z;
	}

	Vector3 Cross(const Vector3& rhs) const
	{
		return Vector3(
			y * rhs.z - z * rhs.y,
			z * rhs.x - x * rhs.z,
			x * rhs.y - y * rhs.x
		);
	}

	float LengthSquared() const
	{
		return x * x + y * y + z * z;
	}

	float Length() const
	{
		return sqrt(LengthSquared());
	}

	void Normalize()
	{
		float length = Length();
		if (length != 0.0f)
		{
			x /= length;
			y /= length;
			z /= length;
		}
	}

	Vector3 Normalized() const
	{
		Vector3 result = *this;
		result.Normalize();
		return result;
	}
};

inline Vector3 operator*(float scalar, const Vector3& vec)
{
	return vec * scalar;
}