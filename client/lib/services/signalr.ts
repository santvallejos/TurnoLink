import * as signalR from '@microsoft/signalr';

export interface BookingNotification {
  type: string;
  bookingId: string;
  clientName: string;
  serviceName: string;
  startTime: string;
  createdAt: string;
  message: string;
}

type NotificationCallback = (notification: BookingNotification) => void;

/**
 * Servicio de SignalR para notificaciones en tiempo real
 */
class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private callbacks: NotificationCallback[] = [];
  private isConnecting = false;

  /**
   * Inicia la conexión con el hub de SignalR
   */
  async start(): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      return;
    }

    if (this.isConnecting) {
      return;
    }

    this.isConnecting = true;

    try {
      const token = this.getToken();
      if (!token) {
        console.warn('No hay token de autenticación para SignalR');
        return;
      }

      const apiUrl = process.env.NEXT_PUBLIC_TURNOLINK_API_URL || 'http://localhost:5000';

      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${apiUrl}/hubs/notifications`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information)
        .build();

      // Registrar el handler para recibir notificaciones
      this.connection.on('ReceiveBookingNotification', (notification: BookingNotification) => {
        this.callbacks.forEach(callback => callback(notification));
      });

      // Eventos de conexión
      this.connection.onreconnecting(() => {
        console.warn('SignalR reconectando...');
      });

      this.connection.onreconnected(() => {
        console.warn('SignalR reconectado');
      });

      this.connection.onclose(() => {
        console.warn('SignalR desconectado');
      });

      await this.connection.start();
    } catch (error) {
      console.error('Error conectando SignalR:', error);
    } finally {
      this.isConnecting = false;
    }
  }

  /**
   * Detiene la conexión con SignalR
   */
  async stop(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }

  /**
   * Registra un callback para recibir notificaciones
   */
  onNotification(callback: NotificationCallback): () => void {
    this.callbacks.push(callback);

    // Retorna función para eliminar el callback
    return () => {
      this.callbacks = this.callbacks.filter(cb => cb !== callback);
    };
  }

  /**
   * Verifica si está conectado
   */
  isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }

  /**
   * Obtiene el token JWT del localStorage
   */
  private getToken(): string | null {
    if (typeof window === 'undefined') return null;
    return localStorage.getItem('turnolink_token');
  }
}

export const signalRService = new SignalRService();
