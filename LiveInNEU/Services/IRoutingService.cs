using System.Threading.Tasks;

namespace LiveInNEU.Services
{
    /// <summary>
    /// ҳ��·�ɽӿ�
    /// </summary>
    public interface IRoutingService
    {
        /// <summary>
        /// ҳ�淵��
        /// </summary>
        Task GoBackAsync();

        /// <summary>
        /// ҳ����ת
        /// </summary>
        /// <param name="route">·��ҳ������</param>
        Task NavigateToAsync(string route);

        // Task NavigateToAsync(string route, params string[] urlParams);
    }
}