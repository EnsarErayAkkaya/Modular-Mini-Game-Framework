# Modular Mini Game Framework

# App Start

This project implements a **modular mini-game framework** in Unity.  
The entry point is handled by the `GameLoader` class, which bootstraps core services and loads initial scenes.

---

## 🔑 How It Works
- **`GameLoader`** is placed in the first scene loaded by Unity.  
- On `Start()`, it:
  1. Initializes the `ServicesContainer` (global service locator).  
  2. Loads the **Initial Scene** (bootstrapping / setup).  
  3. Immediately loads the **Menu Scene**, where players can select mini-games.  

---

## 📂 Project Flow
1. **GameLoader Scene** → Initializes services.  
2. **Initial Scene** → Preloads assets, configs, or splash screens.  
3. **Menu Scene** → Main hub for mini-game selection.  
4. **Mini-Games** → Each game is a separate scene, loaded via the framework.  

---

## ⚙️ Extending
- Add new mini-games as scenes.  
- Register scene keys in `SceneServices.SceneKeys`.  
- The framework will handle loading/unloading smoothly.  

---

## 📝 Notes
- **No hardcoded dependencies** → All systems use the `ServicesContainer`.  
- **Modular & extensible** → Adding new mini-games doesn’t require editing core loader logic.  
- **Scene order independent** → Loader always ensures required services are initialized first.  
