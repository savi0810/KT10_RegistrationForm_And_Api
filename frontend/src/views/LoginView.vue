<template>
  <div class="flex flex-col items-center justify-center gap-4 p-4 min-h-screen">
    <UPageCard class="w-full max-w-md">
      <UAuthForm
        :fields="fields"
        :schema="schema"
        title="Вход"
        description="Введите email и пароль"
        icon="i-lucide-log-in"
        :submit="{ label: 'Войти' }"
        @submit="onSubmit"
      >
        <template #description>
          Нет аккаунта?
          <ULink to="/register" class="text-primary font-medium">Зарегистрироваться</ULink>
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
  { name: 'email', type: 'email', label: 'Email', placeholder: 'your@email.com', required: true },
  { name: 'password', type: 'password', label: 'Пароль', placeholder: '••••••••', required: true }
]

const schema = z.object({
  email: z.string().email('Некорректный email'),
  password: z.string().min(1, 'Пароль обязателен')
})

type Schema = z.output<typeof schema>

const onSubmit = async (event: FormSubmitEvent<Schema>) => {
  try {
    await api.signIn(event.data)
    alert('Успешный вход')
    router.push('/users')
  } catch (err: any) {
    alert(err.response?.data?.message || 'Ошибка входа')
  }
}
</script>