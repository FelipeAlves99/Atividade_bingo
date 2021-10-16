import { api } from './api';

export const initGame = async () => await api.post(`/GameSessions`);

export const drawnNumber = async () => await api.get(`/GameSessions/DrawnNumber`);

export const gameStatus = async () => await api.get(`/GameSessions`);
