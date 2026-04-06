import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:7024/api', 
  headers: { 'Content-Type': 'application/json' }
})

export default {

    async signUp(data: { username: string; email: string; password: string; confirmPassword: string }) {
    const response = await api.post('/account/register', data)
    return response.data
  },

  async signIn(data: { email: string; password: string }) {
    const response = await api.post('/account/login', data)
    return response.data
  },

  async getUsers() {
    const response = await api.get('/users')
    return response.data
  },
  async createUser(data: { username: string; email: string; password: string }) {
    const response = await api.post('/users', data)
    return response.data
  },
  async updateUser(id: number, data: { username: string; email: string; password?: string }) {
    const response = await api.put(`/users/${id}`, data)
    return response.data
  },
  async deleteUser(id: number) {
    await api.delete(`/users/${id}`)
  }
}