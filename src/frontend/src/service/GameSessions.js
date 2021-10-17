import { api } from './api';

export const initGame = async () => await api.post(`/GameSessions`);

export const drawnNumber = async (data) => await api.get(`/GameSessions/DrawnNumber/${data}`);

export const gameStatus = async () => await api.get(`/GameSessions`);
