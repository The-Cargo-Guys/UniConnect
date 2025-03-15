// src/store/index.ts
import { createStore } from 'vuex';

interface State {
  isAuthenticated: boolean;
}

const store = createStore<State>({
  state: {
    isAuthenticated: false,
  },
  mutations: {
    setAuthentication(state, status: boolean) {
      state.isAuthenticated = status;
    },
  },
  actions: {
    login({ commit }) {
      // Perform login logic, then:
      commit('setAuthentication', true);
    },
    logout({ commit }) {
      // Perform logout logic, then:
      commit('setAuthentication', false);
    },
  },
});

export default store;
