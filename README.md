# IN4P
2팀

# IN4P

> Unity 기반 액션 로그라이크(바인딩 오브 아이작 스타일) 팀 프로젝트

[![Unity](https://img.shields.io/badge/Unity-2022.x-000?logo=unity)]()
[![C#](https://img.shields.io/badge/C%23-Game%20Logic-239120?logo=csharp)]()
[![License](https://img.shields.io/badge/License-MIT-blue.svg)]()

---

## 🎯 프로젝트 개요
- **장르**: 2D 탑다운 액션 로그라이크
- **핵심 특징**: 랜덤 맵, 몬스터·보스 AI, 무기(공격) 전환 시스템(전략 패턴)
- **플랫폼**: PC (Windows)
- **엔진/언어**: Unity 2022.x, C#
- **팀 규모**: 3인

> 포트폴리오 핵심: 플레이어 공격(눈물, Brimstone), 공격 방식 전환(Strategy), 보스 패턴, AI FSM, 풀링·GC 최적화

---

## 📸 플레이 데모
![Attack Switch](Docs/images/attack_switch.gif)
![Boss Phase](Docs/images/boss_phase.png)
![Monster AI](Docs/images/mobs.png)

---

## 🔨 주요 기능
- **플레이어 공격 시스템**
  - 기본: 눈물(Tear) 투사체
  - 특수: Brimstone(차지 후 레이저)
  - **전략 패턴** 적용: 런타임에 공격 전환 가능
  - `atkSpeed` ↔ `requiredCharge` 동적 연계
- **AI/FSM**
  - 몬스터: 추적형, 랜덤형, 사격형
  - 보스: HP 구간별 페이즈 전환, 애니메이션 이벤트 기반 타이밍
- **전투 최적화**
  - Object Pooling으로 투사체 생성·파괴 최소화
  - 공격 객체 재사용으로 GC 스파이크 방지

---

## 📂 저장소 구조
