<template>
  <div class="p-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Пользователи</h1>
      <UButton color="red" variant="outline" @click="logout">Выйти</UButton>
    </div>

    <UCard class="mb-6">
      <template #header>
        <h3 class="text-lg font-semibold">Создать пользователя</h3>
      </template>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <UInput v-model="newUser.username" placeholder="Имя пользователя" />
        <UInput v-model="newUser.email" placeholder="Email" />
        <UInput v-model="newUser.password" type="password" placeholder="Пароль" />
      </div>
      <template #footer>
        <UButton color="primary" @click="createUser">Создать</UButton>
      </template>
    </UCard>

    <UTable :columns="columns" :data="users">
      <template #actions-cell="{ row }">
        <UButton color="error" size="xs" @click="deleteUser(row.original.id)">Удалить</UButton>
      </template>
    </UTable>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/services/api'

const router = useRouter()
const users = ref<any[]>([])
const newUser = ref({ username: '', email: '', password: '' })

const columns = [
  { id: 'id', header: 'ID', accessorKey: 'id' },
  { id: 'username', header: 'Имя', accessorKey: 'username' },
  { id: 'email', header: 'Email', accessorKey: 'email' },
  { id: 'actions', header: 'Действия' }
]

const loadUsers = async () => {
  users.value = await api.getUsers()
}

const createUser = async () => {
  await api.createUser(newUser.value)
  newUser.value = { username: '', email: '', password: '' }
  loadUsers()
}

const deleteUser = async (id: number) => {
  if (confirm('Удалить пользователя?')) {
    await api.deleteUser(id)
    loadUsers()
  }
}

const logout = () => {
  router.push('/login')
}

onMounted(loadUsers)
</script>