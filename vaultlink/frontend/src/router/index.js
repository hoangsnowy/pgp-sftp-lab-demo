import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '@/pages/Dashboard.vue'
import Pgp from '@/pages/Pgp.vue'
import SshSftp from '@/pages/SshSftp.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: Dashboard
    },
    {
      path: '/pgp',
      name: 'pgp',
      component: Pgp
    },
    {
      path: '/ssh-sftp',
      name: 'ssh-sftp',
      component: SshSftp
    }
  ]
})

export default router