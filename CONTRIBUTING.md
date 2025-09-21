# 🤝 Contributing to SmartShop-API

Welcome! We're excited that you're interested in contributing to **SmartShop-API**, a .NET-based backend for scalable e-commerce applications. Whether you're fixing bugs, improving documentation, or adding new features, your input is highly valued.

## 🧰 Project Setup

Before contributing, make sure you have:

- [.NET SDK (latest version)](https://dotnet.microsoft.com/download)
- A running PostgreSQL instance
- Git installed

Clone the repo and set up your environment:

```bash
git clone https://github.com/supunsarachitha/SmartShop-API.git
cd SmartShop-API
```

Create your `appsettings.json` in `SmartShop.API/` with your DB connection string.

## 🛠️ How to Contribute

1. **Fork** the repository
2. **Create a branch**  
   `git checkout -b feature/your-feature-name`
3. **Make your changes**
   - Follow C# coding conventions
   - Write unit tests for new logic
   - Keep commits clean and descriptive
4. **Run tests**  
   `dotnet test`
5. **Push your branch**  
   `git push origin feature/your-feature-name`
6. **Open a Pull Request** on GitHub

## 📦 Code Guidelines

- Use meaningful names for variables, methods, and classes
- Keep controllers lean—business logic belongs in services
- Use dependency injection for services and repositories
- Follow RESTful API design principles

## 🧪 Testing

We use `xUnit` for unit testing. Please include unit tests and integration tests for any new features or bug fixes.

## 🛡️ Security

If you discover a vulnerability, please report it via [GitHub Security Advisories](https://github.com/supunsarachitha/SmartShop-API/security/advisories) rather than opening a public issue.

## 📄 License

By contributing, you agree that your contributions will be licensed under the [Apache 2.0 License](LICENSE.txt).

---

Thanks for helping make SmartShop-API better! 💙
