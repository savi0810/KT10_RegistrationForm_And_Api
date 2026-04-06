<template>
  <div class="flex flex-col items-center justify-center gap-4 p-4 min-h-screen">
    <UPageCard class="w-full max-w-md">
      <UAuthForm
        :fields="fields"
        :schema="schema"
        title="Регистрация"
        description="Создайте новый аккаунт"
        icon="i-lucide-user-plus"
        :submit="{ label: 'Зарегистрироваться' }"
        @submit="onSubmit"
      >
        <template #description>
          Уже есть аккаунт?
          <ULink to="/login" class="text-primary font-medium">Войти</ULink>
        </template>
      </UAuthForm>
    </UPageCard>
  </div>
</template>

<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent, AuthFormField } from '@nuxt/ui'
import { useRouter } from 'vue-router'
import api from '@/services/api'

const router = useRouter()

const fields: AuthFormField[] = [
  { name: 'username', type: 'text', label: 'Имя пользователя', placeholder: 'ivan123', required: true },
  { name: 'email', type: 'email', label: 'Email', placeholder: 'ivan@example.com', required: true },
  { name: 'password', type: 'password', label: 'Пароль', placeholder: '••••••••', required: true },
  { name: 'confirmPassword', type: 'password', label: 'Подтверждение пароля', placeholder: '••••••••', required: true }
]

const schema = z.object({
  username: z.string().min(3, 'Минимум 3 символа'),
  email: z.string().email('Некорректный email'),
  password: z.string().min(8, 'Пароль должен быть не менее 8 символов'),
  confirmPassword: z.string()
}).refine(data => data.password === data.confirmPassword, {
  message: 'Пароли не совпадают',
  path: ['confirmPassword']
})

type Schema = z.output<typeof schema>

const onSubmit = async (event: FormSubmitEvent<Schema>) => {
  try {
    await api.signUp(event.data)
    alert('Регистрация успешна! Теперь войдите.')
    router.push('/login')
  } catch (err: any) {
    const data = err.response?.data
    if (data?.errors) {
      const messages = Object.values(data.errors).flat().join(', ')
      alert(messages)
    } else {
      alert(data?.message || 'Ошибка регистрации')
    }
  }
}
</script>