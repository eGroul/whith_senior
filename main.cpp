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
	// 생성자
	Object(const Vector3& pos, const Vector3& fwd, float angle = 120.0f)
		: position(pos), forward(fwd.Normalized()), viewAngle(angle)
	{
		Vector3 worldUp(0.0f, 1.0f, 0.0f);

		right = forward.Cross(worldUp).Normalized();

		up = right.Cross(forward).Normalized();
	}

	// TASK 1: 타겟 방향으로의 단위 벡터 계산
	Vector3 GetDirectionToTarget(const Vector3& targetPosition) const
	{
		Vector3 vec = (targetPosition - position).Normalized();

		if(GetDistanceToTarget(targetPosition) == 0)
			return forward;
		// 설명: 추적 완료한 경우, 즉 Object위치와 대상 위치가 같은 경우에 자신의 현재 방향 그대로 간다. (forward를 리턴)

		return  (targetPosition - position).Normalized();
		// 힌트: 타겟 위치 - 현재 위치를 계산한 후 정규화
	}

	// TASK 2: 현재 위치와 타겟 간의 정확한 거리 계산
	float GetDistanceToTarget(const Vector3& targetPosition) const
	{
		return sqrt(pow(targetPosition.x - position.x, 2) + pow(targetPosition.y - position.y, 2)+ pow(targetPosition.z - position.z, 2));
		// 힌트: 두 점 사이의 거리 공식을 사용
	}

	// TASK 3: 타겟이 오브젝트 기준 왼쪽/오른쪽 판별
	// 반환값: 1(오른쪽), -1(왼쪽), 0(정확히 같은 선상)
	int IsTargetOnRight(const Vector3& targetPosition) const
	{
		return forward.Cross(targetPosition).Normalized().y;
		// 힌트: 외적을 사용하여 방향 판별
		// 설명: 외적의 방향을 구하고 정규화시켜 바로 반환값을 리턴한다.
	}

	// TASK 4: 타겟이 시야각 내에 있는지 판별
	bool IsTargetInViewAngle(const Vector3& targetPosition) const
	{
		return forward.Dot(targetPosition) >= 0 ? true : false;
		// 힌트: 내적을 사용하여 각도 계산
		// 설명: 내적이 0보다 크거나 같은 경우에는 Object 앞에 있어서 시야각에 들어온다고 판단하여 true를 반환한다. (아닌 경우에는 false 반환)
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
	std::cout << "===== 벡터 연산 테스트 =====" << std::endl;

	Vector3 v1(1.0f, 2.0f, 3.0f);
	Vector3 v2(4.0f, 5.0f, 6.0f);

	// TASK 1 : 벡터 출력
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

	// TASK 2 : 벡터의 덧셈, 뺄셈, 곱셈(스칼라 곱)을 출력
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

	// TASK 3 : 벡터의 외적, 내적, 길이를 출력
	{
		std::cout
			<< "v1 · v2: " << dot
			<< std::endl
			<< "v1 × v3: (" << cross.x << ", " << cross.y << ", " << cross.z << ")"
			<< std::endl
			<< "길이(v1): " << length
			<< std::endl;
	}

	Vector3 normalized = v1.Normalized();

	// TASK 4 : 정규화된 벡터와 길이를 출력
	{
		std::cout
			<< "정규화(v1): (" << normalized.x << ", " << normalized.y << ", " << normalized.z << ")"
			<< std::endl
			<< "정규화된 벡터의 길이: " << normalized.Length()
			<< std::endl;
	}
}

void TestTargetTracking()
{
	std::cout << "\n===== 타겟 추적 테스트 =====" << std::endl;

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
		std::cout << "\n대상 위치: (" << target.x << ", " << target.y << ", " << target.z << ")" << std::endl;

		// TASK 1: 방향 벡터 계산 테스트
		Vector3 vec = obj.GetDirectionToTarget(target);
		std::cout << "방향 벡터: (" << vec.x << ", " << vec.y << ", " << vec.z << ")" << std::endl;
		std::cout << "방향 벡터 길이: " << vec.Length() << std::endl;

		// TASK 2: 거리 계산 테스트
		std::cout << "거리: " << obj.GetDistanceToTarget(target) << std::endl;

		// TASK 3: 왼쪽/오른쪽 판별 테스트
		switch (obj.IsTargetOnRight(target))
		{
		case 1:
			std::cout << "타겟 위치: 오른쪽" << std::endl;
			break;
		case -1:
			std::cout << "타겟 위치: 왼쪽" << std::endl;
			break;
		case 0:
			std::cout << "타겟 위치: 정확히 같은 선상" << std::endl;
			break;
		}

		// TASK 4: 시야각 판별 테스트
		std::cout << "시야각 내 여부: " << (obj.IsTargetInViewAngle(target) ? "시야 내" : "시야 밖") << std::endl;
	}
}

int main()
{
	TestVectorOperations();

	TestTargetTracking();

	return 0;
}