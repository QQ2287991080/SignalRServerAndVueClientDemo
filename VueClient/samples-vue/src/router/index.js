import Vue from 'vue'
import VueRouter from 'vue-router'
import Layout from '../views/layout/index'
Vue.use(VueRouter)

export const routes = [
  {
    path: '/',
    component: Layout,
    //redirect: '/dashboard',

    children: [
      {
        path: 'dashboard',
        component: () => import('@/views/Home/index.vue'),
        name: 'Dashboard',
        meta: {
          title: '首页',
          icon: 'dashboard',
        },
      },
    ],
  },
  {
    path: '',
    component: Layout,
    children: [
      {
        path: '/loglist',
        component: () => import('@/views/log'),
        meta: {
          title: '日志列表',
          icon: 'log',
        },
      },
    ],
  },
  {
    path: '',
    component: Layout,
    children: [
      {
        path: '/home',
        component: () => import('@/views/Home.vue'),
        meta: {
          title: '聊天',
          icon: 'dashboard',
        },
      },
    ],
  },
]

const router = new VueRouter({
  routes,
})

export default router
