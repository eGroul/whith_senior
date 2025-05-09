#include <iostream>
#include <cmath>
#include <vector>
#include "Vector.hpp"

class Object
{
private:
	Vector3 position;
	Vector3 forward;
	Vector3 right;
	Vector3 up;
	float viewAngle;

public:
	// ������
	Object(const Vector3& pos, const Vector3& fwd, float angle = 120.0f)
		: position(pos), forward(fwd.Normalized()), viewAngle(angle)
	{
		Vector3 worldUp(0.0f, 1.0f, 0.0f);

		right = forward.Cross(worldUp).Normalized();

		up = right.Cross(forward).Normalized();
	}

	// TASK 1: Ÿ�� ���������� ���� ���� ���
	Vector3 GetDirectionToTarget(const Vector3& targetPosition) const
	{
		Vector3 vec = (targetPosition - position).Normalized();

		if(GetDistanceToTarget(targetPosition) == 0)
			return forward;
		// ����: ���� �Ϸ��� ���, �� Object��ġ�� ��� ��ġ�� ���� ��쿡 �ڽ��� ���� ���� �״�� ����. (forward�� ����)

		return  (targetPosition - position).Normalized();
		// ��Ʈ: Ÿ�� ��ġ - ���� ��ġ�� ����� �� ����ȭ
	}

	// TASK 2: ���� ��ġ�� Ÿ�� ���� ��Ȯ�� �Ÿ� ���
	float GetDistanceToTarget(const Vector3& targetPosition) const
	{
		return sqrt(pow(targetPosition.x - position.x, 2) + pow(targetPosition.y - position.y, 2)+ pow(targetPosition.z - position.z, 2));
		// ��Ʈ: �� �� ������ �Ÿ� ������ ���
	}

	// TASK 3: Ÿ���� ������Ʈ ���� ����/������ �Ǻ�
	// ��ȯ��: 1(������), -1(����), 0(��Ȯ�� ���� ����)
	int IsTargetOnRight(const Vector3& targetPosition) const
	{
		return forward.Cross(targetPosition).Normalized().y;
		// ��Ʈ: ������ ����Ͽ� ���� �Ǻ�
		// ����: ������ ������ ���ϰ� ����ȭ���� �ٷ� ��ȯ���� �����Ѵ�.
	}

	// TASK 4: Ÿ���� �þ߰� ���� �ִ��� �Ǻ�
	bool IsTargetInViewAngle(const Vector3& targetPosition) const
	{
		return forward.Dot(targetPosition) >= 0 ? true : false;
		// ��Ʈ: ������ ����Ͽ� ���� ���
		// ����: ������ 0���� ũ�ų� ���� ��쿡�� Object �տ� �־ �þ߰��� ���´ٰ� �Ǵ��Ͽ� true�� ��ȯ�Ѵ�. (�ƴ� ��쿡�� false ��ȯ)
	}

	const Vector3& GetPosition() const { return position; }

	const Vector3& GetForward() const { return forward; }

	void SetPosition(const Vector3& pos) { position = pos; }

	void SetForward(const Vector3& fwd)
	{
		forward = fwd.Normalized();

		Vector3 worldUp(0.0f, 1.0f, 0.0f);
		right = forward.Cross(worldUp).Normalized();
		up = right.Cross(forward).Normalized();
	}
};

void TestVectorOperations()
{
	std::cout << "===== ���� ���� �׽�Ʈ =====" << std::endl;

	Vector3 v1(1.0f, 2.0f, 3.0f);
	Vector3 v2(4.0f, 5.0f, 6.0f);

	// TASK 1 : ���� ���
	{
		std::cout 
			<< "v1: (" << v1.x << ", " << v1.y << ", " << v1.z << ")" 
			<< std::endl 
			<< "v2: (" << v2.x << ", " << v2.y << ", " << v2.z << ")" 
			<< std::endl;
	}

	Vector3 sum = v1 + v2;
	Vector3 diff = v1 - v2;
	Vector3 scaled = v1 * 2.0f;

	// TASK 2 : ������ ����, ����, ����(��Į�� ��)�� ���
	{
		std::cout
			<< "v1 + v2: (" << sum.x << ", " << sum.y << ", " << sum.z << ")"
			<< std::endl
			<< "v1 - v3: (" << diff.x << ", " << diff.y << ", " << diff.z << ")"
			<< std::endl
			<< "v1 * 2: (" << scaled.x << ", " << scaled.y << ", " << scaled.z << ")"
			<< std::endl;
	}

	float dot = v1.Dot(v2);
	Vector3 cross = v1.Cross(v2);
	float length = v1.Length();

	// TASK 3 : ������ ����, ����, ���̸� ���
	{
		std::cout
			<< "v1 �� v2: " << dot
			<< std::endl
			<< "v1 �� v3: (" << cross.x << ", " << cross.y << ", " << cross.z << ")"
			<< std::endl
			<< "����(v1): " << length
			<< std::endl;
	}

	Vector3 normalized = v1.Normalized();

	// TASK 4 : ����ȭ�� ���Ϳ� ���̸� ���
	{
		std::cout
			<< "����ȭ(v1): (" << normalized.x << ", " << normalized.y << ", " << normalized.z << ")"
			<< std::endl
			<< "����ȭ�� ������ ����: " << normalized.Length()
			<< std::endl;
	}
}

void TestTargetTracking()
{
	std::cout << "\n===== Ÿ�� ���� �׽�Ʈ =====" << std::endl;

	Object obj(Vector3(0.0f, 0.0f, 0.0f), Vector3(0.0f, 0.0f, 1.0f));

	Vector3 targets[] =
	{
		Vector3(5.0f, 0.0f, 5.0f),
		Vector3(-5.0f, 0.0f, 5.0f),
		Vector3(0.0f, 0.0f, -10.0f),
		Vector3(0.0f, 0.0f, 10.0f),
		Vector3(0.0f, 0.0f, 0.0f)
	};

	for (const auto& target : targets)
	{
		std::cout << "\n��� ��ġ: (" << target.x << ", " << target.y << ", " << target.z << ")" << std::endl;

		// TASK 1: ���� ���� ��� �׽�Ʈ
		Vector3 vec = obj.GetDirectionToTarget(target);
		std::cout << "���� ����: (" << vec.x << ", " << vec.y << ", " << vec.z << ")" << std::endl;
		std::cout << "���� ���� ����: " << vec.Length() << std::endl;

		// TASK 2: �Ÿ� ��� �׽�Ʈ
		std::cout << "�Ÿ�: " << obj.GetDistanceToTarget(target) << std::endl;

		// TASK 3: ����/������ �Ǻ� �׽�Ʈ
		switch (obj.IsTargetOnRight(target))
		{
		case 1:
			std::cout << "Ÿ�� ��ġ: ������" << std::endl;
			break;
		case -1:
			std::cout << "Ÿ�� ��ġ: ����" << std::endl;
			break;
		case 0:
			std::cout << "Ÿ�� ��ġ: ��Ȯ�� ���� ����" << std::endl;
			break;
		}

		// TASK 4: �þ߰� �Ǻ� �׽�Ʈ
		std::cout << "�þ߰� �� ����: " << (obj.IsTargetInViewAngle(target) ? "�þ� ��" : "�þ� ��") << std::endl;
	}
}

int main()
{
	TestVectorOperations();

	TestTargetTracking();

	return 0;
}