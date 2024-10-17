# Contributing to Monad-Runner

We're thrilled that you're interested in contributing to Monad-Runner! This document provides guidelines for contributing to the project. By participating in this project, you agree to abide by its terms.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
- [Style Guidelines](#style-guidelines)
- [Commit Messages](#commit-messages)
- [Pull Request Process](#pull-request-process)
- [Additional Resources](#additional-resources)

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to core team!

## Getting Started

1. Fork the repository on GitHub
2. Clone your fork locally
3. Set up the development environment (Unity 2021.3.16f1)
4. Create a branch for your edits

## How to Contribute

1. Ensure any install or build dependencies are removed before the end of the layer when doing a build.
2. Update the README.md with details of changes to the interface, this includes new environment variables, exposed ports, useful file locations and container parameters.
3. Increase the version numbers in any examples files and the README.md to the new version that this Pull Request would represent.
4. You may merge the Pull Request in once you have the sign-off of two other developers, or if you do not have permission to do that, you may request the second reviewer to merge it for you.

## Style Guidelines

### C# Coding Conventions

- Use 4 spaces for indentation
- Follow C# naming conventions:
  - PascalCase for class names, method names, and property names
  - camelCase for local variables and method parameters
  - UPPER_CASE for constants
- Use meaningful and descriptive names for variables, methods, and classes
- Add comments for complex logic or non-obvious code
- Follow the Single Responsibility Principle for classes and methods

### Unity-specific Guidelines

- Use [SerializeField] for inspector-visible private fields
- Prefer composition over inheritance
- Use coroutines for time-based operations
- Optimize performance by using object pooling for frequently instantiated objects

## Commit Messages

- Use the present tense ("Add feature" not "Added feature")
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters or less
- Reference issues and pull requests liberally after the first line

## Pull Request Process

1. Ensure any install or build dependencies are removed before the end of the layer when doing a build.
2. Update the README.md with details of changes to the interface, this includes new environment variables, exposed ports, useful file locations and container parameters.
3. Increase the version numbers in any examples files and the README.md to the new version that this Pull Request would represent.
4. You may merge the Pull Request in once you have the sign-off of two other developers, or if you do not have permission to do that, you may request the second reviewer to merge it for you.

## Additional Resources

- [Unity Documentation](https://docs.unity3d.com/)
- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Unity Design Patterns](https://github.com/Naphier/unity-design-patterns)
---
Thank you for contributing to Monad-Runner!
