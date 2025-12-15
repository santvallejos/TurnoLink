import { defineConfig, globalIgnores } from 'eslint/config';
import nextVitals from 'eslint-config-next/core-web-vitals';
import nextTs from 'eslint-config-next/typescript';

const eslintConfig = defineConfig([
  ...nextVitals,
  ...nextTs,
  // Override default ignores of eslint-config-next.
  globalIgnores([
    // Default ignores of eslint-config-next:
    '.next/**',
    'out/**',
    'build/**',
    'next-env.d.ts',
    'node_modules/**',
  ]),
  {
    // Reglas personalizadas
    rules: {
      // ===== Formato y estilo =====
      // Máximo 1 línea vacía consecutiva
      'no-multiple-empty-lines': ['error', { max: 1, maxEOF: 0, maxBOF: 0 }],
      // Requiere punto y coma
      semi: ['error', 'always'],
      // Comillas simples para strings
      quotes: ['error', 'single', { avoidEscape: true }],
      // Indentación de 2 espacios
      indent: ['error', 2, { SwitchCase: 1 }],
      // Coma final en multilínea
      'comma-dangle': ['error', 'always-multiline'],
      // Sin espacios al final de línea
      'no-trailing-spaces': 'error',
      // Línea vacía al final del archivo
      'eol-last': ['error', 'always'],

      // ===== Mejores prácticas JavaScript =====
      // Sin console.log (warning para desarrollo)
      'no-console': ['warn', { allow: ['warn', 'error'] }],
      // Sin variables no utilizadas
      'no-unused-vars': 'off', // Desactivado en favor de @typescript-eslint
      '@typescript-eslint/no-unused-vars': [
        'error',
        { argsIgnorePattern: '^_', varsIgnorePattern: '^_' },
      ],
      // Sin var, usar let o const
      'no-var': 'error',
      // Preferir const cuando no se reasigna
      'prefer-const': 'error',
      // Sin debugger
      'no-debugger': 'error',

      // ===== React/Next.js =====
      // Requiere key en listas
      'react/jsx-key': 'error',
      // Sin uso de índice como key
      'react/no-array-index-key': 'warn',
      // Ordenar props alfabéticamente (opcional, descomenta si lo deseas)
      // "react/jsx-sort-props": ["warn", { callbacksLast: true, shorthandFirst: true }],
      // Auto-cerrar componentes sin hijos
      'react/self-closing-comp': 'error',
      // Sin fragmentos innecesarios
      'react/jsx-no-useless-fragment': 'warn',

      // ===== Hooks =====
      // Reglas de hooks ya incluidas en nextVitals
      'react-hooks/rules-of-hooks': 'error',
      'react-hooks/exhaustive-deps': 'warn',

      // ===== TypeScript =====
      // Tipos explícitos en funciones públicas (opcional)
      '@typescript-eslint/explicit-function-return-type': 'off',
      // Preferir interfaces sobre types
      '@typescript-eslint/consistent-type-definitions': ['error', 'interface'],
      // Sin any explícito (warning)
      '@typescript-eslint/no-explicit-any': 'warn',

      // ===== Imports =====
      // Sin imports duplicados
      'no-duplicate-imports': 'error',
    },
  },
]);

export default eslintConfig;
